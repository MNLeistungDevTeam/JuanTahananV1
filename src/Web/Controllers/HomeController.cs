using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Interfaces.Setup.ModuleRepository;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Application.Services;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Enums;
using Template.Infrastructure.Persistence.Repositories.Setup.ModuleRepository;
using Template.Web.Controllers.Services;
using Template.Web.Models;

namespace Template.Web.Controllers;

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

    [ModuleServices(ModuleCodes.Home, typeof(IModuleRepository))]
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
    public async Task<IActionResult> GetApplicationsCount() =>
        Ok((await _applicantsPersonalInformationRepo.GetAllAsync()).Count);
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