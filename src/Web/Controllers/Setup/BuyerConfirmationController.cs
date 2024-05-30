using ClosedXML.Excel;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [Route("BuyerConfirmation/BCFRequests/BCFSummary")]
    public async Task<IActionResult> BCFSummary()
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

    public async Task<IActionResult> GetBcf()
    {
        try
        {
            int userId = int.Parse(User.Identity.Name);
            var buyerConfirmation = await _buyerConfirmationRepo.GetByUserAsync(userId);

            return Ok(buyerConfirmation);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    public async Task<IActionResult> BCFSummaryData()
    {
        int userId = int.Parse(User.Identity.Name);
        var userInfo = await _userRepo.GetUserAsync(userId);

        int? roleId = userInfo.UserRoleId.Value;

        int? developerId = roleId == (int)PredefinedRoleType.Developer ? userInfo.PropertyDeveloperId : null;

        var data = await _buyerConfirmationRepo.GetBCDExcelSummaryReportAsync(null, null, developerId);

        return Ok(data);
    }

    public async Task<IActionResult> GetBCFInquiry()
    {
        BuyerConfirmationInqModel bciModel = new();

        int companyId = int.Parse(User.FindFirstValue("Company"));

        int userId = int.Parse(User.Identity.Name);
        var userInfo = await _userRepo.GetUserAsync(userId);

        if (userInfo.UserRoleId == (int)PredefinedRoleType.Developer)
        {
            bciModel = await _buyerConfirmationRepo.GetInqAsync(companyId, userInfo.PropertyDeveloperId);
        }
        else
        {
            bciModel = await _buyerConfirmationRepo.GetInqAsync(companyId, null);
        }

        return Ok(bciModel);
    }

    public async Task<IActionResult> GetMyBCF()
    {
        int userId = int.Parse(User.Identity.Name);

        var data = await _buyerConfirmationRepo.GetByUserAsync(userId);

        return Ok(data);
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
            List<BuyerConfirmationModel> bcModel = new();
            int userId = int.Parse(User.Identity.Name);

            var userInfo = await _userRepo.GetUserAsync(userId);
            var data = await _buyerConfirmationRepo.GetAllAsync();

            if (userInfo.UserRoleId == (int)PredefinedRoleType.Developer)
            {
                bcModel = data.Where(m => m.PropertyDeveloperId == userInfo.PropertyDeveloperId).ToList();
            }
            else
            {
                bcModel = data.ToList();
            }

            return Ok(bcModel);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> DownloadBCFSummary(int? locationId, int? projectId)
    {
        try
        {
            var rootFolder = _webhost.WebRootPath;
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string templatePath = Path.Combine(rootFolder, "Files", "ExcelTemplate", "Prequalified_BuyerConfirmation_Summary.xlsx");

            string? fileName = $"BCFSummary.xlsx";

            int companyId = int.Parse(User.FindFirstValue("Company"));
            int userId = int.Parse(User.Identity.Name);

            var userInfo = await _userRepo.GetUserAsync(userId);

            int? developerId = userInfo.UserRoleId == (int)PredefinedRoleType.Developer ? userInfo.PropertyDeveloperId.Value : null;

            // Get List of Qualified BCF
            var excelList = await _buyerConfirmationRepo.GetBCDExcelSummaryReportAsync(locationId, projectId, developerId);

            int itemCount = excelList.Count();

            // Extract distinct PropertyProjectName values
            var projectName = excelList.Select(x => x.PropertyProjectName).Distinct().FirstOrDefault();
            var developerName = excelList.Select(x => x.PropertyDeveloperName).Distinct().FirstOrDefault();
            var locationName = excelList.Select(x => x.PropertyLocationName).Distinct().FirstOrDefault();

            // Load the Excel template
            using (var workbook = new XLWorkbook(templatePath))
            {
                // Get the first worksheet in the workbook
                var worksheet = workbook.Worksheets.Worksheet(1);

                //Set Date
                worksheet.Cell("G4").Value = DateTime.UtcNow.ToString("yyyy-MM-dd");

                // Example modification: Set the value of cell B5
                worksheet.Cell("B5").Value = developerName;
                worksheet.Cell("B6").Value = projectName;
                worksheet.Cell("B7").Value = locationName;

                worksheet.Cell("L6").Value = itemCount;
                worksheet.Cell("L7").Value = itemCount;

                // Add data
                int startRow = 11; // Starting row for data
                int number = 1;
                foreach (var item in excelList)
                {
                    if (item.FullName is null)
                        break; // Stop if there's no sequence

                    // Populate the cells in the current row
                    var cellA = worksheet.Cell($"A{startRow}");
                    var cellB = worksheet.Cell($"B{startRow}");
                    var cellC = worksheet.Cell($"C{startRow}");
                    var cellD = worksheet.Cell($"D{startRow}");
                    var cellE = worksheet.Cell($"E{startRow}");
                    var cellF = worksheet.Cell($"F{startRow}");
                    var cellG = worksheet.Cell($"G{startRow}");
                    var cellH = worksheet.Cell($"H{startRow}");
                    var cellI = worksheet.Cell($"I{startRow}");
                    var cellJ = worksheet.Cell($"J{startRow}");
                    var cellK = worksheet.Cell($"K{startRow}");
                    var cellL = worksheet.Cell($"L{startRow}");
                    var cellM = worksheet.Cell($"M{startRow}");

                    cellA.Value = $"{number}.";
                    cellB.Value = item.FullName;
                    cellC.Value = item.isPagibigMember ?? "";
                    cellD.Value = item.PagibigNumber ?? "";
                    cellE.Value = item.BirthDate ?? "";
                    cellF.Value = item.MonthlySalary;
                    cellG.Value = string.Join(" ", item.PresentHomeAddress) ?? "";
                    cellH.Value = item.MobileNumber ?? "";
                    cellI.Value = item.Email ?? "";
                    cellJ.Value = item.EmployerName ?? "";
                    cellK.Value = item.EmployerContactPerson ?? "";
                    cellL.Value = item.EmployerContactNumber ?? "";
                    cellM.Value = item.EmployerEmailAddress ?? "";

                    if (startRow > 40)
                    {
                        // Define the range of cells to be styled

                        IXLCell[] cells = { cellA, cellB, cellC, cellD, cellE, cellF, cellG, cellH, cellI, cellJ, cellK, cellL, cellM };

                        foreach (var cell in cells)
                        {
                            setBorderCell(cell);
                        }
                    }

                    startRow++; // Move to the next row
                    number++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

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

    #region Private Methods

    public void setBorderCell(IXLCell cell)
    {
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
    }

    #endregion Private Methods
}