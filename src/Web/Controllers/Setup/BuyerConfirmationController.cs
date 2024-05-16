using ClosedXML.Excel;
using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.SpreadsheetSource;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

    public BuyerConfirmationController(
        IUserRepository userRepo,
        IBeneficiaryInformationRepository beneficiaryInformationRepo,
        IBuyerConfirmationRepository buyerConfirmationRepo)
    {
        _userRepo = userRepo;
        _beneficiaryInformationRepo = beneficiaryInformationRepo;
        _buyerConfirmationRepo = buyerConfirmationRepo;
    }

    #endregion Fields

    #region Views

    public IActionResult ApplicantRequests()
    {
        try
        {
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
            int companyId = int.Parse(User.FindFirstValue("Company"));

            //var vendor = await _fileSetupDA.GetVendors(companyId);

            //if (!vendor.Any())
            //    return BadRequest("No Record Found!");

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"BCFSummary.xlsx";

            using var workbook = new XLWorkbook();
            IXLWorksheet worksheet = workbook.Worksheets.Add("BCFSummary");
            worksheet.Style.Font.FontSize = 9;
            worksheet.Style.Font.FontName = "Arial";




            worksheet.Cell(5, 1).Value = "PROJECT PROPONENT'S NAME:";
            worksheet.Cell(6, 1).Value = "PROJECT NAME:";
            worksheet.Cell(7, 1).Value = "LOCATION:";

            worksheet.Cell(5, 14).Value = "DEVELOPER / CONTRACTOR:";
            worksheet.Cell(6, 14).Value = "ESTIMATED NO. OF UNITS:";
            worksheet.Cell(7, 14).Value = "TOTAL NO. OF PRE-QUALIFIED BENEFICIARIES:";



            worksheet.Cell(9, 1).Value = "Project Beneficiary Details";
            worksheet.Cell(9, 2).Value = "Employment/Business Details";

            worksheet.Cell(9, 3).Value = "No.";

            worksheet.Cell(9, 4).Value = "Name";

            worksheet.Cell(9, 5).Value = "If Pag-IBIG Member \r\n(Yes / No)";

            worksheet.Cell(9, 6).Value = "Pag-IBIG \r\nMID Number";
            worksheet.Cell(9, 7).Value = "Birthday\r\n(mm/dd/yyyy)";
            worksheet.Cell(9, 9).Value = "Gross Monthly Income\r\n(GMI)";
            worksheet.Cell(9, 10).Value = "Present Home Address";
            worksheet.Cell(9, 11).Value = "Contact Number/s";
            worksheet.Cell(9, 12).Value = "Email Address";
            worksheet.Cell(9, 13).Value = "Employer's/Business Name";
            worksheet.Cell(9, 14).Value = "Contact Person";
            worksheet.Cell(9, 15).Value = "Contact Number/s";
            worksheet.Cell(9, 16).Value = "Email Address";




            //int lstndx = 0;
            //int index = 1;

            //List<string> debit_total = new();
            //List<string> credit_total = new();

            //foreach (var item in vendor)
            //{
            //    worksheet.Cell(index + 1, 1).Value = item.VendorCode;
            //    worksheet.Cell(index + 1, 2).Value = item.VendorName;
            //    worksheet.Cell(index + 1, 3).Value = item.CurrentBalance;
            //    worksheet.Cell(index + 1, 4).Value = item.Address?.ToString().Trim().Normalize();
            //    worksheet.Cell(index + 1, 5).Value = item.Representative;
            //    worksheet.Cell(index + 1, 6).Value = item.TransactionCount;
            //    worksheet.Cell(index + 1, 7).Value = item.Inactive ? "Inactive" : "Active";

            //    lstndx = index + 1;

            //    index++;
            //}
            //IXLRange range2 = worksheet.Range(worksheet.Cell(2, 3).Address, worksheet.Cell(lstndx, 3).Address);
            //range2.Style.NumberFormat.Format = "#,##0.00;(#,##0.00);-;";

            //IXLRange range2 = worksheet.Range(worksheet.Cell(2, 2).Address, worksheet.Cell(lstndx, 3).Address);
            //range2.Style.NumberFormat.Format = "#,##0.00;(#,##0.00);-;";

            worksheet.Columns().AdjustToContents();
            worksheet.Rows().AdjustToContents();

            //worksheet.Range($"B3:E{lstndx + 2}").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

            using var stream = new MemoryStream();
              workbook.SaveAs(stream);
            var content =  stream.ToArray();
            return File(content, contentType, fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        // End of Action
    }






    #endregion API Operation
}