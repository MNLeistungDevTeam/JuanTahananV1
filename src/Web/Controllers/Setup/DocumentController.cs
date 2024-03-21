using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Enums;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentTypeRepository _documentTypeRepo;
        private readonly IFileUploadService _uploadService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
        private readonly IBarrowersInformationRepository _barrowersInformationRepo;
        private readonly IDocumentRepository _documentRepo;

        public DocumentController(IDocumentTypeRepository documentTypeRepo, IFileUploadService uploadService, IWebHostEnvironment hostingEnvironment, IUserRepository userRepository, ICurrentUserService currentUserService, IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo, IBarrowersInformationRepository barrowersInformationRepo, IDocumentRepository documentRepo)
        {
            _documentTypeRepo = documentTypeRepo;
            _uploadService = uploadService;
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
            _barrowersInformationRepo = barrowersInformationRepo;
            _documentRepo = documentRepo;
        }

        //[ModuleServices(ModuleCodes.Document, typeof(IModuleRepository))]
        public IActionResult Index()
        {
            return View();
        }

        //[ModuleServices(ModuleCodes.DocumentUpload, typeof(IModuleRepository))]

        [Route("[controller]/DocumentUpload/{applicantCode?}")]
        public async Task<IActionResult> DocumentUpload(string applicantCode = null)
        {
            int userId = 0;
            int applicantId = 0;
            if (applicantCode != null)
            {
                var applicantinfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(applicantCode);

                if (applicantinfo == null)
                {
                    return BadRequest($"{applicantCode}: no record Found!");
                }

                //var user = await _userRepository.GetByIdAsync(applicantinfo.UserId);
                userId = applicantinfo.UserId;
                applicantCode = applicantinfo?.Code;
                applicantId = applicantinfo.Id;
            }

            ViewBag.Id = userId;
            ViewBag.AppplicationId = applicantId;
            ViewBag.ApplicationCode = applicantCode != null ? applicantCode : string.Empty;

            return View();
        }

        public async Task<IActionResult> GetAllUploadedDocuments(int applicationId) =>
            Ok(await _documentRepo.SpGetAllApplicationSubmittedDocuments(applicationId));

        [HttpGet("Document/GetDocumentsByApplicant/{applicantCode}")]
        public async Task<IActionResult> GetDocumentsByApplicant(string applicantCode) =>
           Ok(await _documentTypeRepo.GetByApplicantCodeAsync(applicantCode));

        public async Task<IActionResult> UploadProfile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var user = await _userRepository.GetByIdNoTrackingAsync(_currentUserService.GetCurrentUserId());

                    // Check if the user already has a profile picture and delete it
                    if (!string.IsNullOrEmpty(user.ProfilePicture))
                    {
                        var existingFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "user", user.ProfilePicture);
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            System.IO.File.Delete(existingFilePath);
                        }
                    }

                    // Generate a unique filename for the new profile picture
                    var uniqueFileName = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";
                    user.ProfilePicture = uniqueFileName;

                    // Update the user's profile picture filename in the database
                    await _userRepository.UpdateAsync(user, _currentUserService.GetCurrentUserId());

                    // Construct the file path
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "user", uniqueFileName);

                    // Copy the uploaded file to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Return the URL of the newly uploaded profile picture
                    var imageUrl = $"/images/user/{uniqueFileName}";
                    return Json(new { imageUrl });
                }
                else
                {
                    return BadRequest("No file uploaded or file is empty.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public async Task DocumentUploadFileOnFTP(IFormFile? file, int? ApplicationId, int? DocumentTypeId, int? DocumentId)
        {
            if (file == null || ApplicationId == null || DocumentTypeId == null || DocumentId == null)
            {
                throw new ArgumentNullException("One or more parameters is null.");
            }

            var application = await _applicantsPersonalInformationRepo.GetByIdAsync(ApplicationId.Value);
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(x => x.Id == application?.UserId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            await _uploadService.UploadFiles(
                new List<IFormFile> { file },
                "Ftp_eiDOC2024",
                $"Documents\\{user.FirstName + "-" + user.Id}",
                application.Id,
                DocumentTypeId.Value,
                DocumentId.Value
            );
        }

        public async Task DocumentUploadFile(IFormFile file, int? ApplicationId, int? DocumentTypeId, int? DocumentId)
        {
            try
            {
                if (file == null || ApplicationId == null || DocumentTypeId == null || DocumentId == null)
                {
                    throw new ArgumentNullException("One or more parameters is null.");
                }

                //#region Checker for FileSize maximum 3MB
                long fileSizeInBytes = file.Length;
                double fileSizeInMegabytes = fileSizeInBytes / (1024.0 * 1024.0); // Convert bytes to megabytes

                // Check if the file size exceeds 3MB
                if (fileSizeInMegabytes > 3)
                {
                    throw new Exception("File size exceeds 3MB");
                }

                var application = await _applicantsPersonalInformationRepo.GetAsync(ApplicationId.Value);

                var documentType = await _documentTypeRepo.GetByIdAsync(DocumentTypeId.Value);

                //var users = await _userRepository.GetAllAsync();
                //var user = users.FirstOrDefault(x => x.Id == application?.UserId);

                //if (user == null)
                //{
                //    throw new InvalidOperationException("User not found.");
                //}

                //await _uploadService.UploadFiles(
                //    new List<IFormFile> { file },
                //    "Ftp_eiDOC2024",
                //    $"Documents\\{user.FirstName + "-" + user.Id}",
                //    application.Id,
                //    DocumentTypeId.Value,
                //DocumentId.Value
                //);

                int userId = application.UserId;
                int companyId = application.CompanyId ?? 0;
                int documentTypeId = documentType.Id;

                var rootFolder = _hostingEnvironment.WebRootPath;
                var saveLocation = Path.Combine("Files", "Documents", "Applicant", documentType.Description, application.Code ?? "");
                var referenceType = (int)DocumentReferenceType.Applicant;
                int referenceId = application.Id;

                List<IFormFile> fileList = new List<IFormFile> { file };

                await _uploadService.UploadFilesAsync(fileList, saveLocation, rootFolder, referenceId, application.Code, referenceType, documentTypeId, userId, companyId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task DocumentDelete(int DocumentId)
        {
            //await _uploadService.DeleteFile(DocumentId, "Ftp_eiDOC2024"); for ftp but server not working
            await _uploadService.DeleteFileAsync(DocumentId, _hostingEnvironment.WebRootPath); //use local
        }

        public async Task<IActionResult> GetAllDocumentTypes() =>
            Ok(await _documentTypeRepo.SpGetAllUserDocumentTypes());

        [HttpPost]
        public async Task<IActionResult> Savedocument(DocumentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                                       .Select(x => new
                                       {
                                           Field = x.Key,
                                           DisplayName = ((DisplayNameAttribute)typeof(DocumentTypeModel).GetProperty(x.Key.Split('.').Last())?.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault())?.DisplayName,
                                           Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                                       })
                                       .ToList();

                var errorMessage = new StringBuilder();
                foreach (var error in errors)
                {
                    errorMessage.AppendLine($"Field: {error.DisplayName ?? error.Field.Split(".")[1]} <br>");
                    foreach (var message in error.Messages)
                    {
                        errorMessage.AppendLine($"Error: {message} <br>");
                    }
                }

                return BadRequest(errorMessage.ToString());
            }
            if (model.Document.Id != 0)
            {
                var Documents = await _documentTypeRepo.SpGetAllUserDocumentTypes();
                var Document = Documents.FirstOrDefault(x => x.Id == model.Document.Id);
                Document.Description = model.Document.Description;
                await _documentTypeRepo.SaveAsync(Document);
                return Ok("updated");
            }
            else
            {
                await _documentTypeRepo.SaveAsync(model.Document);
                return Ok("added");
            }
        }

        public async Task<IActionResult> GetDocumentById(int id) =>
            Ok((await _documentTypeRepo.SpGetAllUserDocumentTypes()).FirstOrDefault(x => x.Id == id));

        [HttpDelete]
        public async Task DeleteDocuments(int[] ids) =>
            await _documentTypeRepo.BatchDeleteAsync(ids);

        public async Task<IActionResult> GetFileList(int ApplicationId, int DocumentTypeId) =>
            Ok(await _documentTypeRepo.SpGetAllDocumentsByIds(ApplicationId, DocumentTypeId));

        [Route("[controller]/GetApplicantUploadedDocuments/{applicantCode?}")]
        public async Task<IActionResult> GetApplicantUploadedDocuments(string applicantCode) =>
         Ok(await _documentRepo.GetApplicantDocumentsByCode(applicantCode));

        //[Route("[controller]/GetApplicantUploadedDocumentByDocumentType/{documentTypeId}/{applicantCode?}")]
        public async Task<IActionResult> GetApplicantUploadedDocumentByDocumentType(int documentTypeId, string applicantCode) =>
        Ok(await _documentRepo.GetApplicantDocumentsByDocumentType(documentTypeId, applicantCode));
    }
}