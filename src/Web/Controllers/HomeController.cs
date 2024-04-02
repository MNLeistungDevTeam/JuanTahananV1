using AutoMapper;
using DMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.UserDto;
using DMS.Infrastructure.Persistence.Repositories.Setup.ModuleRepository;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;

namespace DMS.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _userRepo;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;

    public HomeController(ILogger<HomeController> logger, IUserRepository userRepo, ICurrentUserService currentUserService, IMapper mapper, IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
    }

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

            var data = await _applicantsPersonalInformationRepo.GetApplicationInfo(roleId);

            return Ok(data);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //[AllowAnonymous]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    _logger.LogError("Error Occurred");
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}

    [AllowAnonymous]
    public IActionResult Error(ErrorViewModel model)
    {
        try
        {
            return View(model);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }
}