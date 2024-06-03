using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Enums;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DMS.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    #region Fields

    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _userRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
    private readonly IBuyerConfirmationRepository _buyerConfirmationRepo;

    public HomeController(
        ILogger<HomeController> logger,
        IUserRepository userRepo,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
        IBuyerConfirmationRepository buyerConfirmationRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
        _buyerConfirmationRepo = buyerConfirmationRepo;
    }

    #endregion Fields

    #region Views

    //[ModuleServices(ModuleCodes.Home, typeof(IModuleRepository))]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult NotFoundPage()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex });
        }
    }

    [AllowAnonymous]
    public IActionResult BadRequestPage()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex });
        }
    }

    public async Task<IActionResult> BCFDownload()
    {
        try
        {

            int userId = int.Parse(User.Identity.Name);

            var userInfo = await _userRepo.GetUserAsync(userId);

            //if the current user is not beneficiary
            if (userInfo.UserRoleId != (int)PredefinedRoleType.Beneficiary)
            {
                return View("AccessDenied");
            }
            var bcfData = await _buyerConfirmationRepo.GetByUserAsync(userId);

            BuyerConfirmationModel bcModel = new();
            if (bcfData != null)
            {
                if (bcfData.ApprovalStatus != 3)
                {
                    throw new Exception("Your BCF is not yet approved by Developer");
                }

                bcModel = bcfData;
            }

            return View(bcModel);

        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }


    }

    #endregion Views

    public async Task<IActionResult> UpdateThemeUserColor(string color)
    {
        try
        {
            var user = await _userRepo.GetByIdNoTrackingAsync(_currentUserService.GetCurrentUserId());
            if (user != null)
            {
                if (color == null)
                {
                    return Ok((bool)user.IsDark ? "dark" : "light");
                }
                user.IsDark = color == "dark" ? true : false;
                var updated = await _userRepo.UpdateAsync(user, user.Id);
                return Ok((bool)updated.IsDark ? "dark" : "light");
            }
            return BadRequest("Internal Error Cannot Update Theme!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //public async Task<IActionResult> GetApplicationsCount() =>
    //    Ok((await _applicantsPersonalInformationRepo.GetAllAsync()).Count);

    public async Task<IActionResult> GetApplicationsCount()
    {
        try
        {
            int userId = int.Parse(User.Identity.Name);

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;
            string pagibigNumber = userdata.PagibigNumber;

            var data = await _applicantsPersonalInformationRepo.GetApplicationInfo(roleId, pagibigNumber);

            return Ok(data);
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    //[AllowAnonymous]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    _logger.LogError("Error Occurred");
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}

    //[AllowAnonymous]
    //public IActionResult Error(ErrorViewModel model)
    //{
    //    try
    //    {
    //        return View(model);
    //    }
    //    catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    //}

    [AllowAnonymous]
    public IActionResult Error()
    {
        string viewString = "Error";
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            Message = "An error occurred." // Default message
        };

        // Retrieve the exception details, if available
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            errorViewModel.Message = exceptionHandlerPathFeature.Error.Message;

            if (errorViewModel.Message.Contains("Transaction is currently being used"))
            {
                viewString = "TransactionLocked";
            }
        }

        return View(viewString, errorViewModel);
    }
}