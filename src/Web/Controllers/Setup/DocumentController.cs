

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
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Interfaces.Setup.DocumentRepository;
using Template.Application.Interfaces.Setup.ModuleRepository;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Application.Services;
using Template.Domain.Dto.DocumentDto;
using Template.Domain.Enums;
using Template.Web.Controllers.Services;
using Template.Web.Models;

namespace Template.Web.Controllers.Setup
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

        [ModuleServices(ModuleCodes.Document, typeof(IModuleRepository))]
        public IActionResult Index()
        {
            return View();
        }
        [ModuleServices(ModuleCodes.DocumentUpload, typeof(IModuleRepository))]
        public async Task<IActionResult> DocumentUpload(int Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            var info = await _applicantsPersonalInformationRepo.GetbyUserId(user.Id);
            ViewBag.Appplications = info;
            ViewBag.Id = info.UserId;
            ViewBag.Barrowers = await _barrowersInformationRepo.GetByApplicationInfoIdAsync(info.Id);
            ViewBag.ApplicationCode = info.Code;
            return View();
        }

        public async Task<IActionResult> GetAllUploadedDocuments(int applicationId) =>
            Ok(await _documentRepo.SpGetAllApplicationSubmittedDocuments(applicationId));
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
        public async Task DocumentUploadFile(IFormFile? file, int? ApplicationId, int? DocumentTypeId, int? DocumentId)
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
        [HttpDelete]
        public async Task DocumentDelete(int DocumentId)
        {
            await _uploadService.DeleteFile(DocumentId, "Ftp_eiDOC2024");
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
    }
}
