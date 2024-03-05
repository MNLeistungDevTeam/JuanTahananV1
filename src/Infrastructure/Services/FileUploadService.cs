using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.Design;
using Template.Application.Interfaces.Setup.DocumentRepository;
using Template.Application.Services;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;
using Template.Infrastructure.Hubs;

namespace Template.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IFtpDownloaderService _ftpDownloaderService;
        private readonly IHubContext<UploaderHub> _hubContext;
        private readonly IUserDocumentRepository _userDocumentRepo;
        private readonly IMapper _mapper;

        public FileUploadService(IDocumentRepository documentRepository, IFtpDownloaderService ftpDownloaderService, IHubContext<UploaderHub> hubContext, IUserDocumentRepository userDocumentRepo, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _ftpDownloaderService = ftpDownloaderService;
            _hubContext = hubContext;
            _userDocumentRepo = userDocumentRepo;
            _mapper = mapper;
        }

        public async Task UploadFiles(
            List<IFormFile> files,
            string rootPath,
            string saveLocation,
            int ApplicantsPersonalInformationId,
            int DocumentTypeId,
            int DocumentId
            )
        {
            foreach ( var file in files.Where(x => x.Length >= 0) )
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
                    await _ftpDownloaderService.Upload(stream, filePath,progressCallback:async progress =>{
                       await  _hubContext.Clients.All.SendAsync("UpdateSwalProgress", progress);
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
                        if(userDoc == null)
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
                if(userDoc != null)
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
    }
}
