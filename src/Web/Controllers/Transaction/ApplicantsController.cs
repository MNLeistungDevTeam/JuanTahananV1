using AutoMapper;
using DevExpress.Xpo.Helpers;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using DMS.Web.Controllers.Services;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Web.Controllers.Transaction
{
    [Authorize]
    public class ApplicantsController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
        private readonly ILoanParticularsInformationRepository _loanParticularsInformationRepo;
        private readonly ICollateralInformationRepository _collateralInformationRepo;
        private readonly IBarrowersInformationRepository _barrowersInformationRepo;
        private readonly ISpouseRepository _spouseRepo;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authService;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IEmailService _emailService;
        private readonly IDocumentRepository _documentRepo;
        private readonly IForm2PageRepository _form2PageRepo;
        private DMSDBContext _context;

        public ApplicantsController(
            IUserRepository userRepo,
            IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
            ILoanParticularsInformationRepository loanParticularsInformationRepo,
            ICollateralInformationRepository collateralInformationRepo,
            IBarrowersInformationRepository barrowersInformationRepo,
            ISpouseRepository spouseRepo,
            IMapper mapper,
            IAuthenticationService authService,
            IUserRoleRepository userRoleRepo,
            IEmailService emailService,
            IDocumentRepository documentRepo,
            IForm2PageRepository form2PageRepo,
            DMSDBContext context)
        {
            _userRepo = userRepo;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
            _loanParticularsInformationRepo = loanParticularsInformationRepo;
            _collateralInformationRepo = collateralInformationRepo;
            _barrowersInformationRepo = barrowersInformationRepo;
            _spouseRepo = spouseRepo;
            _mapper = mapper;
            _authService = authService;
            _userRoleRepo = userRoleRepo;
            _emailService = emailService;
            _documentRepo = documentRepo;
            _form2PageRepo = form2PageRepo;
            _context = context;
        }

        [ModuleServices(ModuleCodes.Beneficiary, typeof(IModuleRepository))]
        public IActionResult Index()
        {
            return View();
        }

        [ModuleServices(ModuleCodes.HLF068, typeof(IModuleRepository))]
        public async Task<IActionResult> HLF068(int UserId)
        {
            var model = new ApplicantViewModel();
            if (UserId != 0)
            {
                var latestApplicationForm = (await _applicantsPersonalInformationRepo
                    .GetAllAsync())
                    .Where(x => x.UserId == UserId)
                    .OrderByDescending(x => x.DateCreated)
                    .FirstOrDefault() ?? new ApplicantsPersonalInformation()
                    {
                        UserId = UserId
                    };
                model.ApplicantsPersonalInformation = _mapper.Map<ApplicantsPersonalInformationModel>(latestApplicationForm);
                model.LoanParticularsInformation = _mapper.Map<LoanParticularsInformationModel>(await _loanParticularsInformationRepo.GetByApplicationIdAsync(latestApplicationForm.Id)) ?? new();
                model.CollateralInformation = _mapper.Map<CollateralInformationModel>(await _collateralInformationRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
                model.BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(await _barrowersInformationRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
                model.SpouseModel = _mapper.Map<SpouseModel>(await _spouseRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
                model.Form2PageModel = _mapper.Map<Form2PageModel>(await _form2PageRepo.GetByApplicationInfoIdAsync(latestApplicationForm.Id)) ?? new();
            }
            ViewBag.purposeloan = await _context.PurposeOfLoans.Select(x => new { text = x.Description, value = x.Id }).ToListAsync();
            ViewBag.modeOfPayment = await _context.ModeOfPayments.Select(x => new { text = x.Description, value = x.Id }).ToListAsync();
            ViewBag.propertTye = await _context.PropertyTypes.Select(x => new { text = x.Description, value = x.Id }).ToListAsync();
            return View(model);
        }

        [ModuleServices(ModuleCodes.ApplicantRequests, typeof(IModuleRepository))]
        public async Task<IActionResult> ApplicantRequests()
        {
            var items = new List<ApplicantViewModel>();
            foreach (var item in await _applicantsPersonalInformationRepo.GetAllAsync())
            {
                items.Add(new ApplicantViewModel()
                {
                    ApplicantsPersonalInformation = _mapper.Map<ApplicantsPersonalInformationModel>(item),
                    LoanParticularsInformation = _mapper.Map<LoanParticularsInformationModel>(await _loanParticularsInformationRepo.GetByApplicationIdAsync(item.Id)),
                    BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(await _barrowersInformationRepo.GetByApplicationInfoIdAsync(item.Id)),
                    CollateralInformation = _mapper.Map<CollateralInformationModel>(await _collateralInformationRepo.GetByApplicationInfoIdAsync(item.Id)),
                    SpouseModel = _mapper.Map<SpouseModel>(await _spouseRepo.GetByApplicationInfoIdAsync(item.Id)),
                    ApplicationSubmittedDocumentModels = await _documentRepo.SpGetAllApplicationSubmittedDocuments(item.Id)
                });
            }
            ViewBag.items = items;
            return View(items);
        }

        [HttpPost]
       
        public async Task<IActionResult> SaveHLF068(ApplicantViewModel vwModel)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                }


                var user = new User();
                var applicationData = new ApplicantsPersonalInformationModel();

                if (vwModel.ApplicantsPersonalInformation.Id == 0)

                {
                    if (vwModel.ApplicantsPersonalInformation.UserId == 0)
                    {
                        UserModel userModel = new()
                        {
                            Email = vwModel.BarrowersInformationModel.Email,
                            Password = "Pass123$", //default password
                            UserName = await GenerateTemporaryUsernameAsync(),
                            FirstName = vwModel.BarrowersInformationModel.FirstName,
                            LastName = vwModel.BarrowersInformationModel.LastName,
                            Gender = vwModel.BarrowersInformationModel.Sex
                        };

                        //save beneficiary user
                        user = await RegisterBenefeciary(userModel);

                        // reverse map parameter need for email sending
                        var userdata = _mapper.Map<UserModel>(user);

                        await _emailService.SendUserInfo(userdata);
                    }
                    else
                    {
                        user = await _userRepo.GetByIdAsync(vwModel.ApplicantsPersonalInformation.UserId);
                    }

                    applicationData.UserId = user.Id;
                    applicationData.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{user.Id}";
                    applicationData = _mapper.Map<ApplicantsPersonalInformationModel>(await _applicantsPersonalInformationRepo.SaveAsync(applicationData));
                }



                else
                {
                    applicationData = _mapper.Map<ApplicantsPersonalInformationModel>(await _applicantsPersonalInformationRepo.GetByIdAsync(vwModel.ApplicantsPersonalInformation.Id));
                    await _applicantsPersonalInformationRepo.SaveAsync(applicationData.MergeNonNullData(vwModel.ApplicantsPersonalInformation));
                }

                if (vwModel.ApplicantsPersonalInformation.Id != 0)
                {
                    if (user.Id == 0)
                    {
                        user = await _userRepo.GetByIdAsync(vwModel.ApplicantsPersonalInformation.UserId);
                    }
                    vwModel.LoanParticularsInformation.ApplicantsPersonalInformationId = vwModel.ApplicantsPersonalInformation.Id;
                    vwModel.BarrowersInformationModel.ApplicantsPersonalInformationId = vwModel.ApplicantsPersonalInformation.Id;
                    vwModel.CollateralInformation.ApplicantsPersonalInformationId = vwModel.ApplicantsPersonalInformation.Id;
                    vwModel.SpouseModel.ApplicantsPersonalInformationId = vwModel.ApplicantsPersonalInformation.Id;
                    vwModel.Form2PageModel.ApplicantsPersonalInformationId = vwModel.ApplicantsPersonalInformation.Id;

                    var loan = await _loanParticularsInformationRepo.GetByIdAsync(vwModel.LoanParticularsInformation.Id);
                    var collateral = await _collateralInformationRepo.GetByIdAsync(vwModel.CollateralInformation.Id);
                    var barrow = await _barrowersInformationRepo.GetByIdAsync(vwModel.BarrowersInformationModel.Id);
                    var spouse = await _spouseRepo.GetByIdAsync(vwModel.SpouseModel.Id);
                    var form2 = await _form2PageRepo.GetByIdAsync(vwModel.Form2PageModel.Id);
                    if (vwModel.LoanParticularsInformation.Id != 0)
                    {
                        loan.MergeNonNullData(vwModel.LoanParticularsInformation);
                        vwModel.LoanParticularsInformation = _mapper.Map<LoanParticularsInformationModel>(loan);
                    }
                    if (vwModel.CollateralInformation.Id != 0)
                    {
                        collateral.MergeNonNullData(vwModel.CollateralInformation);
                        vwModel.CollateralInformation = _mapper.Map<CollateralInformationModel>(collateral);
                    }
                    if (vwModel.BarrowersInformationModel.Id != 0)
                    {
                        barrow.MergeNonNullData(vwModel.BarrowersInformationModel);
                        vwModel.BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(barrow);
                    }
                    if (vwModel.SpouseModel.Id != 0)
                    {
                        spouse.MergeNonNullData(vwModel.SpouseModel);
                        vwModel.SpouseModel = _mapper.Map<SpouseModel>(spouse);
                    }
                    if (vwModel.Form2PageModel.Id != 0)
                    {
                        form2.MergeNonNullData(vwModel.Form2PageModel);
                        vwModel.Form2PageModel = _mapper.Map<Form2PageModel>(form2);
                    }
                    await _loanParticularsInformationRepo.SaveAsync(vwModel.LoanParticularsInformation);
                    await _barrowersInformationRepo.SaveAsync(vwModel.BarrowersInformationModel);
                    await _collateralInformationRepo.SaveAsync(vwModel.CollateralInformation);

                    await _spouseRepo.SaveAsync(vwModel.SpouseModel);
                    await _form2PageRepo.SaveAsync(vwModel.Form2PageModel);
                }

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<string> GenerateTemporaryUsernameAsync()
        {
            string temporaryUsername;
            int index = 1;

            do
            {
                temporaryUsername = $"Beneficiary-{index++}";
            } while (await UsernameExistsAsync(temporaryUsername));

            return temporaryUsername;
        }

        private async Task<bool> UsernameExistsAsync(string username)
        {
            return (await _userRepo.GetAllAsync()).Any(x => x.UserName == username);
        }

        [ModelStateValidations(typeof(UserModel))]
        public async Task<User> RegisterBenefeciary(UserModel user)
        {
            user.Position = "Beneficiary";

            // validate and  register user
            var userData = await _authService.RegisterUser(user);

            //save as benificiary
            await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

            return userData;
        }

        public async Task<IActionResult> GetUsersByRoleName(string roleName) =>
            Ok(await _userRepo.spGetByRoleName(roleName));

        public async Task<IActionResult> GetPurposeOfLoan() =>
            Ok(await _context.PurposeOfLoans.Select(x => new { text = x.Description, value = x.Id }).ToListAsync());

        public async Task<IActionResult> GetModeOfPayment() =>
            Ok(await _context.ModeOfPayments.Select(x => new { text = x.Description, value = x.Id }).ToListAsync());

        public async Task<IActionResult> GetPropertyType() =>
            Ok(await _context.PropertyTypes.Select(x => new { text = x.Description, value = x.Id }).ToListAsync());
    }
}