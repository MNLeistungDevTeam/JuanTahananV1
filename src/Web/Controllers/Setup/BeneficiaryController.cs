using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

public class BeneficiaryController : Controller
{
    private readonly IUserRepository _userRepo;
    private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
    private readonly INotificationService _notificationService;
    private readonly IAuthenticationService _authenticationService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IEmailService _emailService;
    private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
    private readonly IRoleAccessRepository _roleAccessRepo;

    public BeneficiaryController(
        IUserRepository userRepo,
        IBeneficiaryInformationRepository beneficiaryInformationRepo,
        INotificationService notificationService,
        IAuthenticationService authenticationService,
        IBackgroundJobClient backgroundJobClient,
        IUserRoleRepository userRoleRepo,
        IEmailService emailService,
        IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
        IRoleAccessRepository roleAccessRepo)
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
    }

    #region Views

    public async Task <IActionResult> Index()
    {
        var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_BENEFICIARY_MGMT);

        if (roleAccess is null) { return View("AccessDenied"); }
        if (!roleAccess.CanRead) { return View("AccessDenied"); }

        return View();
    }

    [Route("[controller]/Details/{pagibigNumber?}")]
    public async Task<IActionResult> Details(string? pagibigNumber = null)
    {
        var vwModel = new BeneficiaryInformationModel();

        var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);
        var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(pagibigNumber);

        //beneficiaryData.ProfilePicture = beneficiaryData.ProfilePicture ?? string.Empty;

        if (beneficiaryData != null)
        {
            vwModel = beneficiaryData;
        }

        return View(vwModel);
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
                    Password = _authenticationService.GenerateTemporaryPasswordAsync(model.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                    UserName = await _authenticationService.GenerateTemporaryUsernameAsync(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Sex,
                    Position = "Beneficiary",
                    PagibigNumber = model.PagibigNumber
                };

                // validate and  register user
                var userData = await _authenticationService.RegisterUser(userModel);

                userModel.Id = userData.Id;

                model.UserId = userData.Id;
                //save as benificiary
                await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

                userModel.Action = "created";
                //// make the usage of hangfire
                _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential(userModel));

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

    #endregion API Operation
}