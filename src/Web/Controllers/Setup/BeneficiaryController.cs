using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.UserDto;

using DMS.Domain.Enums;
using DMS.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class BeneficiaryController : Controller
{
    #region Fields

    private readonly IUserRepository _userRepo;
    private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
    private readonly INotificationService _notificationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IEmailService _emailService;
    private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
    private readonly IRoleAccessRepository _roleAccessRepo;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICompanyRepository _companyRepo;
    private readonly IPropertyUnitRepository _propertyUnitRepo;
    private readonly IPropertyProjectRepository _propertyProjectRepo;
    private readonly IPropertyLocationRepository _propertyLocationRepo;

    public BeneficiaryController(
        IUserRepository userRepo,
        IBeneficiaryInformationRepository beneficiaryInformationRepo,
        INotificationService notificationService,
        IAuthenticationService authenticationService,
        IBackgroundJobClient backgroundJobClient,
        IUserRoleRepository userRoleRepo,
        IEmailService emailService,
        IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
        IRoleAccessRepository roleAccessRepo,
        IWebHostEnvironment webHostEnvironment,
        ICompanyRepository companyRepo,
        IPropertyUnitRepository propertyUnitRepo,
        IPropertyProjectRepository propertyProjectRepo,
        IPropertyLocationRepository propertyLocationRepo)
    {
        _userRepo = userRepo;
        _beneficiaryInformationRepo = beneficiaryInformationRepo;
        _notificationService = notificationService;
        _authenticationService = authenticationService;
        _backgroundJobClient = backgroundJobClient;
        _userRoleRepo = userRoleRepo;
        _emailService = emailService;
        _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
        _roleAccessRepo = roleAccessRepo;
        _webHostEnvironment = webHostEnvironment;
        _companyRepo = companyRepo;
        _propertyUnitRepo = propertyUnitRepo;
        _propertyProjectRepo = propertyProjectRepo;
        _propertyLocationRepo = propertyLocationRepo;
    }

    #endregion Fields

    #region Views

    public async Task<IActionResult> Index()
    {
        var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_BENEFICIARY_MGMT);

        if (roleAccess is null) { return View("AccessDenied"); }
        if (!roleAccess.CanRead) { return View("AccessDenied"); }

        return View();
    }

    [Route("[controller]/Details/{pagibigNumber?}")]
    public async Task<IActionResult> Details(string? pagibigNumber = null)
    {
        try
        {
            var vwModel = new BeneficiaryInformationModel();

            //var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);

            int userId = int.Parse(User.Identity.Name);
            var userInfo = await _userRepo.GetUserAsync(userId);

            var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(pagibigNumber);

            if (beneficiaryData == null)
            {
                throw new Exception($"Transaction No: {pagibigNumber}: no record Found!");
            }

            //if the application is not access by beneficiary
            if (beneficiaryData.UserId != userId && userInfo.UserRoleId == 4)
            {
                return View("AccessDenied");
            }

            //beneficiaryData.ProfilePicture = beneficiaryData.ProfilePicture ?? string.Empty;

            vwModel = beneficiaryData;

            return View(vwModel);
        }
        catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
    }

    [Route("[controller]/BeneficiaryInformation/{pagibigNumber?}")]
    public async Task<IActionResult> Beneficiary(string? pagibigNumber = null)
    {
        var vwModel = new BeneficiaryInformationModel();

        //var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);
        var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(pagibigNumber);

        if (beneficiaryData != null)
        {
            vwModel = beneficiaryData;
        }

        return View(vwModel);
    }

    #endregion Views

    #region API Operation

    [HttpPost]
    public async Task<IActionResult> SaveBeneficiary(BeneficiaryInformationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
            }

            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));
            model.CompanyId = companyId;

            model.PagibigNumber = model.PagibigNumber.Replace("-", "");

            //create  new beneficiary

            if (model.UserId == 0)
            {
                #region Create Beneficiary User

                //Create Beneficiary User
                UserModel userModel = new()
                {
                    Email = model.Email,
                    Password = _authenticationService.GenerateRandomPassword(), /*_authenticationService.GenerateTemporaryPasswordAsync(model.FirstName),*/ //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                    UserName = await _authenticationService.GenerateTemporaryUsernameAsync(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Sex,
                    Position = "Beneficiary",
                    PagibigNumber = model.PagibigNumber,
                    CompanyId = companyId,
                };

                // validate and  register user
                var userData = await _authenticationService.RegisterUser(userModel);

                userModel.Id = userData.Id;

                model.UserId = userData.Id;
                //save as benificiary
                await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

                userModel.Action = "created";

                userModel.SenderId = userId;

                //// make the usage of hangfire
                _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential2(userModel, _webHostEnvironment.WebRootPath));

                #endregion Create Beneficiary User
            }

            var beneficiaryData = await _beneficiaryInformationRepo.SaveAsync(model, userId);

            // last stage pass parameter code

            #region Notification

            var type = model.Id == 0 ? "Added" : "Updated";
            var actiontype = type;

            var actionlink = $"Beneficiary/Details/{beneficiaryData.PagibigNumber}";

            await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_BENEFICIARY_MGMT, actionlink, actiontype, beneficiaryData.PagibigNumber, userId, companyId);

            #endregion Notification

            return Ok(beneficiaryData.PagibigNumber);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetDevelopers()
    {
        var companies = await _companyRepo.GetCompanies();
        var filteredCompanies = companies
      .Where(company => company.Id != 1 && company.Code != "JTH-PH")
      .ToList();

        return Ok(filteredCompanies);
    }

    public async Task<IActionResult> GetProjects(int? developerId)
    {
        if (developerId is null)
        {
            var data = await _propertyProjectRepo.GetAll();
            return Ok(data);
        }
        else
        {
            var data = await _propertyProjectRepo.GetByCompanyAsync(developerId.Value, null);
            return Ok(data);
        }
    }

    public async Task<IActionResult> GetUnits(int? projectId, int? developerId)
    {
        if (projectId is null)
        {
            var data = await _propertyUnitRepo.GetAll();
            return Ok(data);
        }
        else
        {
            var data = await _propertyUnitRepo.GetUnitByProjectAsync(projectId.Value, developerId);

            return Ok(data);
        }
    }

    public async Task<IActionResult> GetLocations(int? projectId,int? developerId)
    {
        if (projectId is null)
        {
            var data = await _propertyLocationRepo.GetAll();
            return Ok(data);
        }
        else
        {
            var data = await _propertyLocationRepo.GetPropertyLocationByProjectAsync(projectId.Value,developerId) ;

            return Ok(data);
        }
    }

    #endregion API Operation
}