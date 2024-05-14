using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

    #endregion API Operation
}