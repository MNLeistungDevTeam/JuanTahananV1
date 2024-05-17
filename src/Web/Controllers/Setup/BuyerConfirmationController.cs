using ClosedXML.Excel;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class BuyerConfirmationController : Controller
{
    #region Fields

    private readonly IUserRepository _userRepo;
    private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
    private readonly IBuyerConfirmationRepository _buyerConfirmationRepo;
    private readonly IWebHostEnvironment _webhost;

    public BuyerConfirmationController(
        IUserRepository userRepo,
        IBeneficiaryInformationRepository beneficiaryInformationRepo,
        IBuyerConfirmationRepository buyerConfirmationRepo, IWebHostEnvironment webhost)
    {
        _userRepo = userRepo;
        _beneficiaryInformationRepo = beneficiaryInformationRepo;
        _buyerConfirmationRepo = buyerConfirmationRepo;
        _webhost = webhost;
    }

    #endregion Fields

    #region Views

    public async Task<IActionResult> ApplicantRequests()
    {
        try
        {
            int userId = int.Parse(User.Identity.Name);

            var userInfo = await _userRepo.GetUserAsync(userId);

            if (userInfo.UserRoleId != (int)PredefinedRoleType.Developer)
            {
                return View("AccessDenied");
            }

            return View();
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBCF(ApplicantViewModel vwModel)
    {
        try
        {
            BuyerConfirmationModel bcfModel = new();

            int userId = int.Parse(User.Identity.Name);

            var bnfInfo = await _buyerConfirmationRepo.GetByCodeAsync(vwModel.BuyerConfirmationModel.Code);

            bcfModel = bnfInfo;
            bcfModel.MonthlyAmortization = vwModel.BuyerConfirmationModel.MonthlyAmortization;
            bcfModel.SellingPrice = vwModel.BuyerConfirmationModel.SellingPrice;

            await _buyerConfirmationRepo.SaveAsync(bcfModel, userId);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Route("[controller]/LatestBCF")]
    public async Task<IActionResult> LatestBCF()
    {
        try
        {
            ApplicantViewModel vwModel = new();

            int userId = int.Parse(User.Identity.Name);

            var userData = await _userRepo.GetUserAsync(userId);

            var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(userData.PagibigNumber);

            vwModel.BuyerConfirmationModel.UserId = userData.Id;
            vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

            vwModel.BuyerConfirmationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
            vwModel.BuyerConfirmationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
            vwModel.BuyerConfirmationModel.LastName = beneficiaryData.LastName ?? string.Empty;

            vwModel.BuyerConfirmationModel.Email = beneficiaryData.Email;

            vwModel.BuyerConfirmationModel.Suffix = userData.Suffix;

            return View(vwModel);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    [Route("[controller]/Details/{bcfCode}")]
    public async Task<IActionResult> Details(string bcfCode)
    {
        if (string.IsNullOrEmpty(bcfCode))
        {
            return View("Error", new ErrorViewModel { Message = "Error: no code provided!" });
        }

        try
        {
            int userId = int.Parse(User.Identity.Name);

            var userInfo = await _userRepo.GetUserAsync(userId);

            if (userInfo.UserRoleId != (int)PredefinedRoleType.Developer)
            {
                return View("AccessDenied");
            }

            var buyerConfirmation = await _buyerConfirmationRepo.GetByCodeAsync(bcfCode);

            var viewModel = new ApplicantViewModel()
            {
                BuyerConfirmationModel = buyerConfirmation
            };

            return View("Details", viewModel);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    public async Task<IActionResult> Upload()
    {
        try
        {
            int userId = int.Parse(User.Identity.Name);
            var buyerConfirmation = await _buyerConfirmationRepo.GetByUserAsync(userId);

            return View(buyerConfirmation);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    #endregion Views

    #region API Getters

    public async Task<IActionResult> GetBCFInquiry()
    {
        int companyId = int.Parse(User.FindFirstValue("Company"));

        var result = await _buyerConfirmationRepo.GetInqAsync(companyId);
        return Ok(result);
    }

    public async Task<IActionResult> GetBCFapplicationByCode(string code)
    {
        var result = await _buyerConfirmationRepo.GetByCodeAsync(code);

        return Ok(result);
    }

    #endregion API Getters

    #region API Operation

    [HttpPost]
    public async Task<IActionResult> SaveBCF(ApplicantViewModel vwModel)
    {
        try
        {
            //if (!ModelState.IsValid)
            //{
            //    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
            //}

            int userId = int.Parse(User.Identity.Name);

            //Unmasked
            vwModel.BuyerConfirmationModel.PagibigNumber = vwModel.BuyerConfirmationModel.PagibigNumber.Replace("-", "") ?? string.Empty;

            var bcfData = await _buyerConfirmationRepo.SaveAsync(vwModel.BuyerConfirmationModel, userId);

            return Ok(bcfData.Code);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetBcfList()
    {
        try
        {
            var data = await _buyerConfirmationRepo.GetAllAsync();
            return Ok(data);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> BCFSummary()
    {
        try
        {
            var rootFolder = _webhost.WebRootPath;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string templatePath = Path.Combine(rootFolder, "Files", "ExcelTemplate", "Prequalified_BuyerConfirmation_Summary.xlsx");

            string fileName = $"BCFSummary.xlsx";

            int companyId = int.Parse(User.FindFirstValue("Company"));

            // Load the Excel template
            using (var workbook = new XLWorkbook(templatePath))
            {
                // Get the first worksheet in the workbook
                var worksheet = workbook.Worksheets.Worksheet(1);

                // Example modification: Set the value of cell A1
                worksheet.Cell("B5").Value = "Yuhum Residences";

                // Save the modified workbook to a MemoryStream
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    var content = memoryStream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        // End of Action
    }

    #endregion API Operation
}