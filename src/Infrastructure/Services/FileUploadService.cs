using AutoMapper;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.StaticFiles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DMS.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFtpDownloaderService _ftpDownloaderService;
        private readonly IHubContext<UploaderHub> _hubContext;
        private readonly IUserDocumentRepository _userDocumentRepo;
        private readonly IMapper _mapper;
        private readonly IDocumentTypeRepository _documentTypeRepo;

        public FileUploadService(IDocumentRepository documentRepository,
            IFtpDownloaderService ftpDownloaderService,
            IHubContext<UploaderHub> hubContext,
            IUserDocumentRepository userDocumentRepo,
            IMapper mapper,
            IDocumentTypeRepository documentTypeRepo)
        {
            _documentRepository = documentRepository;
            _ftpDownloaderService = ftpDownloaderService;
            _hubContext = hubContext;
            _userDocumentRepo = userDocumentRepo;
            _mapper = mapper;
            _documentTypeRepo = documentTypeRepo;
        }

        #region FTP Base Directory File Upload

        public async Task UploadFiles(
            List<IFormFile> files,
            string rootPath,
            string saveLocation,
            int ApplicantsPersonalInformationId,
            int DocumentTypeId,
            int DocumentId
            )
        {
            foreach (var file in files.Where(x => x.Length >= 0))
            {
                if (!(file.Length <= 100 * 1024 * 1024)) // Check if file size exceeds 100 MB
                {
                    throw new ArgumentException("File size exceeds the allowed limit of 100 MB.");
                }
                using (var stream = file.OpenReadStream())
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string rawFilePath = Path.Combine(saveLocation, uniqueFileName);
                    string filePath = Path.Combine(rootPath, rawFilePath);
                    var existingDocument = await _documentRepository.GetById(DocumentId);

                    if (existingDocument != null)
                    {
                        await _ftpDownloaderService.DeleteFileAsync(existingDocument.Location);
                        await _documentRepository.DeleteAsync(existingDocument.Id);
                    }
                    await _ftpDownloaderService.Upload(stream, filePath, progressCallback: async progress =>
                    {
                        await _hubContext.Clients.All.SendAsync("UpdateSwalProgress", progress);
                    });
                    Document document = new()
                    {
                        ReferenceId = 0,
                        ReferenceNo = "",
                        Code = uniqueFileName,
                        Name = file.FileName,
                        Location = filePath,
                        Size = (int)file.Length,
                        FileType = file.ContentType,
                        IsFolder = false,
                        CompanyId = 1,
                        DocumentTypeId = DocumentTypeId
                    };
                    if (existingDocument != null)
                    {
                        var userDoc = await _userDocumentRepo.GetByDocumentIdAsync(existingDocument.Id);
                        if (userDoc == null)
                        {
                            throw new Exception();
                        }
                        existingDocument.Id = 0;
                        existingDocument.ReferenceNo = "";
                        existingDocument.DocumentTypeId = DocumentTypeId;
                        existingDocument.Code = uniqueFileName;
                        existingDocument.Name = file.FileName;
                        existingDocument.Location = filePath;
                        existingDocument.Size = (int)file.Length;
                        existingDocument.FileType = file.ContentType;
                        existingDocument.IsFolder = false;
                        existingDocument.CompanyId = 1;
                        existingDocument.DocumentTypeId = DocumentTypeId;
                        var generated = await _documentRepository.UpdateAsync(existingDocument);
                        userDoc.DocumentId = generated.Id;
                        await _userDocumentRepo.UpdateAsync(_mapper.Map<UserDocumentModel>(userDoc));
                    }
                    else
                    {
                        var docs = await _documentRepository.CreateAsync(document);
                        await _userDocumentRepo.CreateAsync(new UserDocumentModel()
                        {
                            DocumentId = docs.Id,
                            DocumentTypeId = DocumentTypeId,
                            ApplicantsPersonalInformationId = ApplicantsPersonalInformationId
                        });
                    }
                }
            }
        }

        public async Task DeleteFile(int documentId, string rootFolder)
        {
            var document = await _documentRepository.GetById(documentId);

            if (document != null)
            {
                var userDoc = await _userDocumentRepo.GetByDocumentIdAsync(documentId);
                if (userDoc != null)
                {
                    var filePath = Path.Combine(rootFolder, document.Location);
                    await _ftpDownloaderService.DeleteFileAsync(document.Location);
                    await _userDocumentRepo.DeleteAsync(userDoc.Id);
                    await _documentRepository.DeleteAsync(documentId);
                }
            }
        }

        public async Task BatchDeleteFile(int[] documentIds, string rootFolder)
        {
            var documents = await _documentRepository.GetByIds(documentIds);

            foreach (var document in documents)
            {
                if (document is null)
                {
                    continue;
                }
                var filePath = Path.Combine(rootFolder, document.Location);
                // Delete the file from the FTP server
                await _ftpDownloaderService.DeleteFileAsync(document.Location);

                // Delete the corresponding Document entity from the database
                await _documentRepository.DeleteAsync(document);
            }
        }

        #endregion FTP Base Directory File Upload

        #region Application Base Directory File Upload

        public async Task UploadFilesAsync(
            List<IFormFile>? files,
            string saveLocation,
            string rootPath,
            int referenceId,
            string referenceNo,
            int referenceType,
            int documentTypeId,
            int userId,
            int companyId)
        {
            try
            {
                if (files == null)
                    return;

                foreach (var formFile in files)
                {
                    if (formFile.Length <= 0)
                    {
                        continue;
                    }

                    //// Generate a unique filename for the uploaded file
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                    //// Combine the save location with the unique filename

                    //using (FileStream stream = new(filePath, FileMode.Create))
                    //{
                    //    //await formFile.CopyToAsync(stream);
                    //}

                    // Create a new Document entity to save in the database

                    string fileName = await ExistingFileCheck(Path.Combine(rootPath, saveLocation, formFile.FileName));
                    Directory.CreateDirectory(Path.Combine(rootPath, saveLocation));

                    using var stream = new FileStream(Path.Combine(rootPath, saveLocation, fileName), FileMode.Create);
                    await formFile.CopyToAsync(stream);

                    string rawFilePath = $"/{saveLocation.Replace("\\", "/")}/";
                    string filePath = Path.Combine(rootPath, rawFilePath);
                    // return $"/{saveLocation.Replace("\\", "/")}/{fileName}";
                    var provider = new FileExtensionContentTypeProvider();
                    string contentType = string.Empty;

                    provider.TryGetContentType(fileName, out contentType);

                    string extension = Path.GetExtension(fileName).ToLower();

                    Document document = new()
                    {
                        ReferenceId = referenceId,
                        ReferenceNo = referenceNo,
                        ReferenceTypeId = referenceType,
                        Code = uniqueFileName,
                        Name = formFile.FileName,
                        Location = filePath,
                        Size = (int)formFile.Length,
                        DocumentTypeId = documentTypeId,
                        FileType = contentType,
                        IsFolder = false,
                        CompanyId = companyId,
                        CreatedById = userId
                    };

                    //File Type validation
                    var documentType = await _documentTypeRepo.GetByIdAsync(documentTypeId);
                    FileType fileType = (FileType)documentType.FileType;

                    string convertedToString = $".{fileType.ToString().ToLower()}";

                    if (extension == ".jpeg" || extension == ".jpg")
                    {
                        extension = ".jpg";
                    }

                    if (extension != convertedToString)
                        throw new ArgumentException("Invalid Assigned File Type.");

                    await _documentRepository.CreateAsync(document);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteFileAsync(int documentId, string rootFolder)
        {
            var document = await _documentRepository.GetById(documentId);

            if (document != null)
            {
                //var filePath = Path.Combine(rootFolder, document.Location, document.Name);

                string filePath = string.Format("{0}{1}{2}", rootFolder, document.Location.Replace("/", "\\"), document.Name);

                // Delete the file from the file system
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Delete the corresponding Document entity from the database
                await _documentRepository.DeleteAsync(documentId);
            }
        }

        public async Task BatchDeleteFileAsync(int[] documentIds, string rootFolder)
        {
            var documents = await _documentRepository.GetByIds(documentIds);

            foreach (var document in documents)
            {
                if (document is null)
                {
                    continue;
                }
                var filePath = Path.Combine(rootFolder, document.Location);
                // Delete the file from the file system
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Delete the corresponding Document entity from the database
                await _documentRepository.DeleteAsync(document);
            }
        }

        public async Task<string?> SaveFileAsync(IFormFile? file, string location, string rootPath)
        {
            try
            {
                if (file == null)
                {
                    return null;
                }

                string fileName = await ExistingFileCheck(Path.Combine(rootPath, location, file.FileName));
                Directory.CreateDirectory(Path.Combine(rootPath, location));

                using var stream = new FileStream(Path.Combine(rootPath, location, fileName), FileMode.Create);
                await file.CopyToAsync(stream);

                return $"/{location.Replace("\\", "/")}/{fileName}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string?> SaveProfilePictureAsync(IFormFile? file, string userName, string location, string rootPath)
        {
            try
            {
                if (file == null)
                {
                    return null;
                }

                var fileName = userName + Path.GetExtension(file.FileName);
                var path = Path.Combine(rootPath, location, fileName);

                //for testing only, the uploaded picture will overwrite the existing image
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                Directory.CreateDirectory(Path.Combine(rootPath, location));
                using var image = Image.Load(file.OpenReadStream());
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(200, 200),
                    Mode = ResizeMode.Crop
                }));
                await image.SaveAsync(path);

                return $"/{location.Replace("\\", "/")}/{fileName}";
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region Private Helper Methods

        private async Task<string> ExistingFileCheck(string fileNameLocation)
        {
            await Task.Yield();

            if (File.Exists(fileNameLocation))
            {
                string extension = Path.GetExtension(fileNameLocation);
                string directory = Path.GetDirectoryName(fileNameLocation);
                string randomFileName = GenerateRandomFileName(extension);
                string randomFilePath = Path.Combine(directory, randomFileName);

                return await ExistingFileCheck(randomFilePath);
            }
            else
            {
                return Path.GetFileName(fileNameLocation);
            }
        }

        private string GenerateRandomFileName(string extension)
        {
            const int length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            var randomChars = Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray();

            return $"{new string(randomChars)}{extension}";
        }

        #endregion Private Helper Methods
    }

    #endregion Application Base Directory File Upload
}