using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Interfaces.Setup.DocumentVerification;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

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
    private readonly IDocumentVerificationRepository _documentVerificationRepo;
    private readonly IRoleAccessRepository _currentUserRoleAccessService;

    public DocumentController(IDocumentTypeRepository documentTypeRepo,
        IFileUploadService uploadService,
        IWebHostEnvironment hostingEnvironment,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
        IBarrowersInformationRepository barrowersInformationRepo,
        IDocumentRepository documentRepo,
        IDocumentVerificationRepository documentVerificationRepo,
        IRoleAccessRepository currentUserRoleAccessService)
    {
        _documentTypeRepo = documentTypeRepo;
        _uploadService = uploadService;
        _hostingEnvironment = hostingEnvironment;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
        _barrowersInformationRepo = barrowersInformationRepo;
        _documentRepo = documentRepo;
        _documentVerificationRepo = documentVerificationRepo;
        _currentUserRoleAccessService = currentUserRoleAccessService;
    }

    #region Views

    //[ModuleServices(ModuleCodes.Document, typeof(IModuleRepository))]
    public async Task<IActionResult> Index()
    {
        try
        {
            var roleAccess = await _currentUserRoleAccessService.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_DOCUMENT);

            if (roleAccess == null || !roleAccess.CanRead)
                return View("AccessDenied");

            //if () { return View("AccessDenied"); }

            ViewData["RoleAccess"] = roleAccess;

            return View("Index");
        }
        catch (Exception)
        {
            throw;
        }
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

    #endregion Views

    #region API Getters

    public async Task<IActionResult> GetAllUploadedDocuments(int applicationId) =>
        Ok(await _documentRepo.SpGetAllApplicationSubmittedDocuments(applicationId));

    [HttpGet("Document/GetDocumentsByApplicant/{applicantCode}")]
    public async Task<IActionResult> GetDocumentsByApplicant(string applicantCode) =>
        Ok(await _documentTypeRepo.GetByApplicantCodeAsync(applicantCode));

    [Route("[controller]/GetApplicantUploadedDocuments/{applicantCode?}")]
    public async Task<IActionResult> GetApplicantUploadedDocuments(string applicantCode) =>
        Ok(await _documentRepo.GetApplicantDocumentsByCode(applicantCode));

    public async Task<IActionResult> GetApplicantUploadedDocumentByDocumentType(int documentTypeId, string applicantCode) =>
        Ok(await _documentRepo.GetApplicantDocumentsByDocumentType(documentTypeId, applicantCode));

    public async Task<IActionResult> GetEligibilityApplicationDocument(string? applicantCode)
    {
        int type = 1; // eligibility documents
        var data = await _documentVerificationRepo.GetByTypeAsync(type, applicantCode);

        return Ok(data);
    }

    public async Task<IActionResult> GetApplicantApplicationDocument(string? applicantCode)
    {
        int type = 2; // eligibility documents
        var data = await _documentVerificationRepo.GetByTypeAsync(type, applicantCode);

        return Ok(data);
    }

    public async Task<IActionResult> GetAllEligibilityDocumentSetup()
    {
        int type = 1; // eligibility documents
        var data = await _documentVerificationRepo.GetByTypeAsync(type, null);

        return Ok(data);
    }

    public async Task<IActionResult> GetAllApplicationDocumentSetup()
    {
        int type = 2; // eligibility documents
        var data = await _documentVerificationRepo.GetByTypeAsync(type, null);

        return Ok(data);
    }

    public async Task<IActionResult> GetDocumentById(int id) =>
        Ok((await _documentTypeRepo.SpGetAllUserDocumentTypes()).FirstOrDefault(x => x.Id == id));

    public async Task<IActionResult> GetDocumentTypeById(int id) =>
        Ok(await _documentTypeRepo.GetDocumentTypeById(id));

    public async Task<IActionResult> GetFileList(int ApplicationId, int DocumentTypeId) =>
        Ok(await _documentTypeRepo.SpGetAllDocumentsByIds(ApplicationId, DocumentTypeId));

    public async Task<IActionResult> GetAllDocumentTypes() =>
        Ok(await _documentTypeRepo.SpGetAllUserDocumentTypes());

    public async Task<IActionResult> GetAllDocumentType() =>
        Ok(await _documentTypeRepo.GetInquiryAsync());

    #endregion API Getters

    #region API Operation

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

    public async Task<IActionResult> DocumentUploadFile(IFormFile file, int? ApplicationId, int? DocumentTypeId, int? DocumentId)
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
            var saveLocation = Path.Combine("Files", "Documents", "Applicant", application.Code, documentType.Code ?? "");
            var referenceType = (int)DocumentReferenceType.Applicant;
            int referenceId = application.Id;

            List<IFormFile> fileList = new List<IFormFile> { file };

            await _uploadService.UploadFilesAsync(fileList, saveLocation, rootFolder, referenceId, application.Code, referenceType, documentTypeId, userId, companyId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SaveDocumentType(DocumentViewModel vwModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
                return Conflict(errors);
            }

            int userId = int.Parse(User.Identity.Name);
            var documentType = await _documentTypeRepo.SaveAsync(vwModel.DocumentType);

            if (vwModel.DocumentVerification.Type != null)
            {
                var docuVerif = await _documentVerificationRepo.GetByDocumentTypeId(vwModel.DocumentType.Id);

                vwModel.DocumentVerification.Id = docuVerif != null ? docuVerif.Id : 0;
                vwModel.DocumentVerification.DocumentTypeId = documentType.Id;
                vwModel.DocumentVerification.Type = vwModel.DocumentVerification.Type;
                vwModel.DocumentVerification.DocumentTypeDescription = documentType.Description;

                await _documentVerificationRepo.SaveAsync(vwModel.DocumentVerification, userId);
            }

            //if (model.DocumentType.Id != 0)
            //{
            //    var Documents = await _documentTypeRepo.SpGetAllUserDocumentTypes();
            //    var Document = Documents.FirstOrDefault(x => x.Id == model.DocumentType.Id);
            //    Document.Description = model.DocumentType.Description;
            //    await _documentTypeRepo.SaveAsync(Document);
            //    return Ok("updated");
            //}
            //else
            //{
            //    await _documentTypeRepo.SaveAsync(model.DocumentType);
            //    return Ok("added");
            //}

            return Ok("added");
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpDelete]
    public async Task DocumentDelete(int DocumentId)
    {
        //await _uploadService.DeleteFile(DocumentId, "Ftp_eiDOC2024"); for ftp but server not working
        await _uploadService.DeleteFileAsync(DocumentId, _hostingEnvironment.WebRootPath); //use local
    }

    [HttpDelete]
    public async Task DeleteDocuments(string ids)
    {
        int[] _ids = Array.ConvertAll(ids.Split(','), int.Parse);
        int[] verifIds = _ids.Select(id =>
        {
            var docuVerif = _documentVerificationRepo.GetByDocumentTypeId(id);
            return docuVerif != null ? docuVerif.Id : -1;
        }).ToArray();

        await _documentVerificationRepo.BatchDeleteAsync(verifIds);
        await _documentTypeRepo.BatchDeleteAsync(_ids);
    }

    [HttpDelete]
    public async Task DeleteDocumentType(string ids)
    {
        int[] _ids = Array.ConvertAll(ids.Split(','), int.Parse);

        await _documentTypeRepo.BatchDeleteAsync2(_ids);
    }

    #endregion API Operation
}