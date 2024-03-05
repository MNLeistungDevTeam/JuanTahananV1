using AutoMapper;
using DevExpress.Xpo.Helpers;
using DevExpress.XtraRichEdit.Import.Doc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Interfaces.Setup.DocumentRepository;
using Template.Application.Interfaces.Setup.ModuleRepository;
using Template.Application.Interfaces.Setup.RoleRepository;
using Template.Application.Interfaces.Setup.UserRepository;
using Template.Application.Services;
using Template.Domain.Common;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;
using Template.Domain.Enums;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Repositories.Setup.UserRepository;
using Template.Infrastructure.Services;
using Template.Web.Controllers.Services;
using Template.Web.Models;

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
        private MNLTemplateDBContext _context;

        public ApplicantsController(IUserRepository userRepo, IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo, ILoanParticularsInformationRepository loanParticularsInformationRepo, ICollateralInformationRepository collateralInformationRepo, IBarrowersInformationRepository barrowersInformationRepo, ISpouseRepository spouseRepo, IMapper mapper, IAuthenticationService authService, IUserRoleRepository userRoleRepo, IEmailService emailService, IDocumentRepository documentRepo, IForm2PageRepository form2PageRepo, MNLTemplateDBContext context)
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
                    .FirstOrDefault() ?? new ApplicantsPersonalInformation() { 
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
            foreach(var item in await _applicantsPersonalInformationRepo.GetAllAsync())
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
        [ModelStateValidations(typeof(ApplicantViewModel))]
        public async Task<IActionResult> SaveHLF068(ApplicantViewModel model)
        {
            try
            {
                var user = new User();
                var applicationData = new ApplicantsPersonalInformationModel();
                if (model.ApplicantsPersonalInformation.Id == 0)
                {

                    if (model.ApplicantsPersonalInformation.UserId == 0)
                    {
                        user = await RegisterBenefeciary(new UserModel()
                        {
                            Email = model.BarrowersInformationModel.Email,
                            Password = "Pass123$", //default password
                            UserName = await GenerateTemporaryUsernameAsync(),
                            FirstName = model.BarrowersInformationModel.FirstName,
                            LastName = model.BarrowersInformationModel.LastName,
                            Gender = model.BarrowersInformationModel.Sex
                        });
                        await _emailService.SendUserInfo(_mapper.Map<UserModel>(user));
                    }
                    else
                    {
                        user = await _userRepo.GetByIdAsync(model.ApplicantsPersonalInformation.UserId);
                    }
                    applicationData.UserId = user.Id;
                    applicationData.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{user.Id}";
                    applicationData = _mapper.Map<ApplicantsPersonalInformationModel>(await _applicantsPersonalInformationRepo.SaveAsync(applicationData));
                }
                else
                {
                    applicationData = _mapper.Map<ApplicantsPersonalInformationModel>(await _applicantsPersonalInformationRepo.GetByIdAsync((int)model.ApplicantsPersonalInformation.Id));
                    await _applicantsPersonalInformationRepo.SaveAsync(applicationData.MergeNonNullData(model.ApplicantsPersonalInformation));
                }
                if (model.ApplicantsPersonalInformation.Id != 0)
                {
                    if(user.Id == 0)
                    {
                        user = await _userRepo.GetByIdAsync(model.ApplicantsPersonalInformation.UserId);
                    }
                    model.LoanParticularsInformation.ApplicantsPersonalInformationId = model.ApplicantsPersonalInformation.Id;
                    model.BarrowersInformationModel.ApplicantsPersonalInformationId = model.ApplicantsPersonalInformation.Id;
                    model.CollateralInformation.ApplicantsPersonalInformationId = model.ApplicantsPersonalInformation.Id;
                    model.SpouseModel.ApplicantsPersonalInformationId = model.ApplicantsPersonalInformation.Id;
                    model.Form2PageModel.ApplicantsPersonalInformationId = model.ApplicantsPersonalInformation.Id;


                    var loan = await _loanParticularsInformationRepo.GetByIdAsync(model.LoanParticularsInformation.Id);
                    var collateral = await _collateralInformationRepo.GetByIdAsync(model.CollateralInformation.Id);
                    var barrow = await _barrowersInformationRepo.GetByIdAsync(model.BarrowersInformationModel.Id);
                    var spouse = await _spouseRepo.GetByIdAsync(model.SpouseModel.Id);
                    var form2 = await _form2PageRepo.GetByIdAsync(model.Form2PageModel.Id);
                    if (model.LoanParticularsInformation.Id != 0)
                    {
                        loan.MergeNonNullData(model.LoanParticularsInformation);
                        model.LoanParticularsInformation = _mapper.Map<LoanParticularsInformationModel>(loan);
                    }
                    if (model.CollateralInformation.Id != 0)
                    {
                        collateral.MergeNonNullData(model.CollateralInformation);
                        model.CollateralInformation = _mapper.Map<CollateralInformationModel>(collateral);
                    }
                    if (model.BarrowersInformationModel.Id != 0)
                    {
                        barrow.MergeNonNullData(model.BarrowersInformationModel);
                        model.BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(barrow);
                    }
                    if (model.SpouseModel.Id != 0)
                    {
                        spouse.MergeNonNullData(model.SpouseModel);
                        model.SpouseModel = _mapper.Map<SpouseModel>(spouse);
                    }
                    if(model.Form2PageModel.Id != 0)
                    {
                        form2.MergeNonNullData(model.Form2PageModel);
                        model.Form2PageModel = _mapper.Map<Form2PageModel>(form2);
                    }
                    await _loanParticularsInformationRepo.SaveAsync(model.LoanParticularsInformation);
                    await _barrowersInformationRepo.SaveAsync(model.BarrowersInformationModel);
                    await _collateralInformationRepo.SaveAsync(model.CollateralInformation);
                    await _spouseRepo.SaveAsync(model.SpouseModel);
                    await _form2PageRepo.SaveAsync(model.Form2PageModel);
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
            var userData = await _authService.RegisterUser(user);
            await _userRoleRepo.CreateAsync(new UserRoleModel()
            {
                UserId = userData.Id,
                RoleId = 4 //BeneficiaryRole
            });
            return userData;
        }
        public async Task<IActionResult> GetUsersByRoleName(string roleName) =>
            Ok(await _userRepo.spGetByRoleName(roleName));

        public async Task<IActionResult> GetPurposeOfLoan() =>
            Ok(await _context.PurposeOfLoans.Select(x => new {text = x.Description ,value = x.Id}).ToListAsync());
        public async Task<IActionResult> GetModeOfPayment() =>
            Ok(await _context.ModeOfPayments.Select(x => new { text = x.Description, value = x.Id }).ToListAsync());
        public async Task<IActionResult> GetPropertyType() =>
            Ok(await _context.PropertyTypes.Select(x => new { text = x.Description, value = x.Id }).ToListAsync());
    }
}
