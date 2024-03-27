﻿using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.Interfaces.Setup.DocumentVerification;
using DMS.Application.Interfaces.Setup.ModeOfPaymentRepo;
using DMS.Application.Interfaces.Setup.PropertyTypeRepo;
using DMS.Application.Interfaces.Setup.PurposeOfLoanRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.SourcePagibigFundRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using DMS.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        private readonly IPurposeOfLoanRepository _purposeOfLoanRepo;
        private readonly IModeOfPaymentRepository _modeOfPaymentRepo;
        private readonly IPropertyTypeRepository _propertyTypeRepo;
        private readonly INotificationService _notificationService;
        private readonly IApprovalService _approvalService;
        private readonly ISourcePagibigFundRepository _sourcePagibigFundRepo;
        private readonly IDocumentVerificationRepository _documentVerificationRepo;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRoleAccessRepository _roleAccessRepo;

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
            DMSDBContext context,
            IPurposeOfLoanRepository purposeOfLoanRepo,
            IModeOfPaymentRepository modeOfPaymentRepo,
            IPropertyTypeRepository propertyTypeRepo,
            INotificationService notificationService,
            IApprovalService approvalService,
            ISourcePagibigFundRepository sourcePagibigFundRepo,
            IDocumentVerificationRepository documentVerificationRepo,
            IBackgroundJobClient backgroundJobClient,
            IRoleAccessRepository roleAccessRepo)
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

            _purposeOfLoanRepo = purposeOfLoanRepo;
            _modeOfPaymentRepo = modeOfPaymentRepo;
            _propertyTypeRepo = propertyTypeRepo;
            _notificationService = notificationService;
            _approvalService = approvalService;
            _sourcePagibigFundRepo = sourcePagibigFundRepo;
            _documentVerificationRepo = documentVerificationRepo;
            _backgroundJobClient = backgroundJobClient;
            _roleAccessRepo = roleAccessRepo;
        }

        #region View

        //[ModuleServices(ModuleCodes.Beneficiary, typeof(IModuleRepository))]

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        [Route("[controller]/Beneficiary")]
        public async Task<IActionResult> Beneficiary()
        {
            int userId = int.Parse(User.Identity.Name);

            var userData = await _userRepo.GetUserAsync(userId);

            var applicationRecord = await _applicantsPersonalInformationRepo.GetAllApplicationsByPagibigNumber(userData.PagibigNumber.ToString());
            var latestRecord = applicationRecord.OrderByDescending(m => m.Code).ThenBy(m => m.DateModified).FirstOrDefault();

            ApplicantViewModel viewModel = new();

            ApplicantsPersonalInformationModel applicantInfoModel = new();
            applicantInfoModel.PagibigNumber = userData.PagibigNumber;
            applicantInfoModel.UserId = userData.Id;

            if (latestRecord != null && latestRecord.ApprovalStatus == 2 || latestRecord.ApprovalStatus == 5)
            {
                applicantInfoModel.isCanAppliedNewApplication = true;
            }

            viewModel.ApplicantsPersonalInformationModel = applicantInfoModel;
            return View(viewModel);
        }

        [Route("[controller]/Details/{applicantCode}")]
        public async Task<IActionResult> Details(string applicantCode)
        {
            try
            {
                //check if applicant code not null go to edit form

                var applicantinfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(applicantCode);

                var barrowerInfo = await _barrowersInformationRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (applicantinfo == null)
                {
                    return BadRequest($"{applicantCode}: no record Found!");
                }

                var eligibilityPhaseDocument = await _documentVerificationRepo.GetByTypeAsync(1, applicantCode);

                var incompleteDocumentData = eligibilityPhaseDocument.Where(dv => dv.TotalDocumentCount == 0).ToList();

                applicantinfo.isRequiredDocumentsUploaded = false;

                if (incompleteDocumentData.Count > 0)
                {
                    applicantinfo.isRequiredDocumentsUploaded = true;
                }

                var viewModel = new ApplicantViewModel()
                {
                    ApplicantsPersonalInformationModel = applicantinfo,
                    BarrowersInformationModel = barrowerInfo,
                };

                return View(viewModel);
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        [Route("[controller]/HLF068/{applicantCode?}")]
        public async Task<IActionResult> HLF068(string? applicantCode = null)
        {
            var vwModel = new ApplicantViewModel();

            //check if applicant code not null go to edit form
            if (applicantCode != null)
            {
                var applicantinfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(applicantCode);

                if (applicantinfo == null)
                {
                    return BadRequest($"{applicantCode}: no record Found!");
                }
                vwModel.ApplicantsPersonalInformationModel = applicantinfo;

                var applicantloaninfo = await _loanParticularsInformationRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (applicantloaninfo != null)
                {
                    vwModel.LoanParticularsInformationModel = applicantloaninfo;
                    vwModel.ApplicantsPersonalInformationModel.UserId = applicantinfo.UserId;
                    vwModel.ApplicantsPersonalInformationModel.Code = applicantinfo.Code;
                }

                var spouseInfo = await _spouseRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (spouseInfo != null)
                {
                    vwModel.SpouseModel = spouseInfo;
                }

                var borrowerInfo = await _barrowersInformationRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (borrowerInfo != null)
                {
                    vwModel.BarrowersInformationModel = borrowerInfo;
                }

                var collateralInfo = await _collateralInformationRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (collateralInfo != null)
                {
                    vwModel.CollateralInformationModel = collateralInfo;
                }

                var form2PageInfo = await _form2PageRepo.GetByApplicantIdAsync(applicantinfo.Id);

                if (form2PageInfo != null)
                {
                    vwModel.Form2PageModel = form2PageInfo;
                }
            }

            return View(vwModel);
        }

        public async Task<IActionResult> ApplicantRequests()
        {
            try
            {
                var roleAccess = await _roleAccessRepo.GetCurrentUserRoleAccessByModuleAsync(ModuleCodes2.CONST_APPLICANTSREQUESTS);

                if (roleAccess is null) { return View("AccessDenied"); }
                if (!roleAccess.CanRead) { return View("AccessDenied"); }

                ViewData["RoleAccess"] = roleAccess;

                //var items = new List<ApplicantViewModel>();

                //foreach (var item in await _applicantsPersonalInformationRepo.GetAllAsync())
                //{
                //    items.Add(new ApplicantViewModel()
                //    {
                //        ApplicantsPersonalInformationModel = _mapper.Map<ApplicantsPersonalInformationModel>(item),
                //        LoanParticularsInformationModel = _mapper.Map<LoanParticularsInformationModel>(await _loanParticularsInformationRepo.GetByApplicationIdAsync(item.Id)),
                //        BarrowersInformationModel = _mapper.Map<BarrowersInformationModel>(await _barrowersInformationRepo.GetByApplicationInfoIdAsync(item.Id)),
                //        CollateralInformationModel = _mapper.Map<CollateralInformationModel>(await _collateralInformationRepo.GetByApplicationInfoIdAsync(item.Id)),
                //        SpouseModel = _mapper.Map<SpouseModel>(await _spouseRepo.GetByApplicationInfoIdAsync(item.Id)),
                //        ApplicationSubmittedDocumentModels = await _documentRepo.SpGetAllApplicationSubmittedDocuments(item.Id)
                //    });
                //}
                //ViewBag.items = items;

                return View();
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        [Route("[controller]/NewHLF068/{pagibigNumber?}")]
        public async Task<IActionResult> NewHLF068(string? pagibigNumber = null)
        {
            var vwModel = new ApplicantViewModel();

            var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);

            vwModel.ApplicantsPersonalInformationModel.UserId = userData.Id;

            vwModel.BarrowersInformationModel.FirstName = userData.FirstName ?? string.Empty;
            vwModel.BarrowersInformationModel.MiddleName = userData.MiddleName ?? string.Empty;
            vwModel.BarrowersInformationModel.LastName = userData.LastName ?? string.Empty;

            vwModel.BarrowersInformationModel.Email = userData.Email;

            vwModel.ApplicantsPersonalInformationModel.PagibigNumber = userData.PagibigNumber;
            vwModel.BarrowersInformationModel.Sex = userData.Gender;
            vwModel.BarrowersInformationModel.Suffix = userData.Suffix;

            return View(vwModel);
        }

        #endregion View

        #region Get Methods

        public async Task<IActionResult> GetUsersByRoleName(string roleName) =>
            Ok(await _userRepo.spGetByRoleName(roleName));

        public async Task<IActionResult> GetPurposeOfLoan() =>
            Ok(await _purposeOfLoanRepo.GetAllAsync());

        public async Task<IActionResult> GetModeOfPayment() =>
            Ok(await _modeOfPaymentRepo.GetAllAsync());

        public async Task<IActionResult> GetPropertyType() =>
            Ok(await _propertyTypeRepo.GetAllAsync());

        public async Task<IActionResult> GetApplicants()

        {
            int userId = int.Parse(User.Identity.Name);

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var data = await _applicantsPersonalInformationRepo.GetApplicantsAsync(roleId);
            return Ok(data);
        }

        public async Task<IActionResult> GetMyApplications()
        {
            int userId = int.Parse(User.Identity.Name);

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var applicationData = await _applicantsPersonalInformationRepo.GetApplicantsAsync(roleId);

            var beneficiaryApplicationData = applicationData.Where(item => item.UserId == userId);

            return Ok(beneficiaryApplicationData);
        }

        public async Task<IActionResult> GetApplicantData(int id)
        {
            var data = await _applicantsPersonalInformationRepo.GetAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetSpouseByApplicantInfoData(int id)
        {
            var data = await _spouseRepo.GetByApplicationInfoIdAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetCollateralByApplicantInfoData(int id)
        {
            var data = await _collateralInformationRepo.GetByApplicationInfoIdAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetBarrowerByApplicantInfoData(int id)
        {
            var data = await _barrowersInformationRepo.GetByApplicationInfoIdAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetForm2ByApplicantInfoData(int id)
        {
            var data = await _form2PageRepo.GetByApplicationInfoIdAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetLoanParticularsByApplicantInfoData(int id)
        {
            var data = await _loanParticularsInformationRepo.GetByApplicationIdAsync(id);
            return Ok(data);
        }

        public async Task<IActionResult> GetApprovalTotalInfo()
        {
            //int companyId = int.Parse(User.FindFirstValue("Company"));
            //int userId = int.Parse(User.Identity.Name);

            return Ok(await _applicantsPersonalInformationRepo.GetApprovalTotalInfo(null));
        }

        public async Task<IActionResult> GetBeneficiaryApprovalTotalInfo()
        {
            //int companyId = int.Parse(User.FindFirstValue("Company"));
            int userId = int.Parse(User.Identity.Name);

            return Ok(await _applicantsPersonalInformationRepo.GetApprovalTotalInfo(userId));
        }

        public async Task<IActionResult> GetEligibilityVerificationDocuments(string applicantCode)
        {
            var data = await _applicantsPersonalInformationRepo.GetEligibilityVerificationDocuments(applicantCode);
            return Ok(data);
        }

        public async Task<IActionResult> GetApplicationVerificationDocuments(string applicantCode)
        {
            var data = await _applicantsPersonalInformationRepo.GetApplicationVerificationDocuments(applicantCode);
            return Ok(data);
        }

        public async Task<IActionResult> GetAllSourcePagibigFund()
        {
            var data = await _sourcePagibigFundRepo.GetAllAsync();
            return Ok(data);
        }

        #endregion Get Methods

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
                // var applicationData = new ApplicantsPersonalInformationModel();
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));

                //create  new beneficiary

                if (vwModel.ApplicantsPersonalInformationModel.Id == 0)

                {
                    vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;

                    #region Register User and Send Email

                    if (vwModel.ApplicantsPersonalInformationModel.UserId == 0)
                    {
                        UserModel userModel = new()
                        {
                            Email = vwModel.BarrowersInformationModel.Email,
                            Password = GeneratePassword(vwModel.BarrowersInformationModel.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                            UserName = await GenerateTemporaryUsernameAsync(),
                            FirstName = vwModel.BarrowersInformationModel.FirstName,
                            LastName = vwModel.BarrowersInformationModel.LastName,
                            Gender = vwModel.BarrowersInformationModel.Sex
                        };

                        //save beneficiary user
                        user = await RegisterBenefeciary(userModel);

                        //// reverse map parameter need for email sending
                        //var userdata = _mapper.Map<UserModel>(user);

                        // make the usage of hangfire
                        userModel.Action = "created";
                        _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential(userModel));
                    }
                    else
                    {
                        user = await _userRepo.GetByIdAsync(vwModel.ApplicantsPersonalInformationModel.UserId);

                        vwModel.BarrowersInformationModel.FirstName = user.FirstName ?? string.Empty;
                        vwModel.BarrowersInformationModel.MiddleName = user.MiddleName ?? string.Empty;
                        vwModel.BarrowersInformationModel.LastName = user.LastName ?? string.Empty;

                        vwModel.BarrowersInformationModel.Email = user.Email;

                        vwModel.ApplicantsPersonalInformationModel.PagibigNumber = user.PagibigNumber;
                        vwModel.BarrowersInformationModel.Sex = user.Gender;
                        vwModel.BarrowersInformationModel.Suffix = user.Suffix;
                    }

                    #endregion Register User and Send Email

                    vwModel.ApplicantsPersonalInformationModel.UserId = user.Id;
                    vwModel.ApplicantsPersonalInformationModel.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{user.Id}";

                    vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;

                    var newApplicantData = await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    if (vwModel.BarrowersInformationModel != null)
                    {
                        vwModel.BarrowersInformationModel.ApplicantsPersonalInformationId = newApplicantData.Id;

                        await _barrowersInformationRepo.SaveAsync(vwModel.BarrowersInformationModel);
                    }

                    if (vwModel.CollateralInformationModel != null && vwModel.CollateralInformationModel.PropertyTypeId != null)
                    {
                        vwModel.CollateralInformationModel.ApplicantsPersonalInformationId = newApplicantData.Id;

                        await _collateralInformationRepo.SaveAsync(vwModel.CollateralInformationModel);
                    }

                    if (vwModel.LoanParticularsInformationModel != null)
                    {
                        vwModel.LoanParticularsInformationModel.ApplicantsPersonalInformationId = newApplicantData.Id;

                        await _loanParticularsInformationRepo.SaveAsync(vwModel.LoanParticularsInformationModel, userId);
                    }

                    if (vwModel.SpouseModel != null && vwModel.SpouseModel.FirstName != null)
                    {
                        vwModel.SpouseModel.ApplicantsPersonalInformationId = newApplicantData.Id;

                        //vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        //vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

                        await _spouseRepo.SaveAsync(vwModel.SpouseModel);
                    }

                    if (vwModel.Form2PageModel != null)
                    {
                        vwModel.Form2PageModel.ApplicantsPersonalInformationId = newApplicantData.Id;

                        await _form2PageRepo.SaveAsync(vwModel.Form2PageModel);
                    }
                }

                //edit saving all data
                else
                {
                    vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;

                    await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    user.Id = vwModel.ApplicantsPersonalInformationModel.UserId;

                    if (vwModel.BarrowersInformationModel != null)
                    {
                        await _barrowersInformationRepo.SaveAsync(vwModel.BarrowersInformationModel);
                    }

                    if (vwModel.CollateralInformationModel != null && vwModel.CollateralInformationModel.PropertyTypeId != null)
                    {
                        await _collateralInformationRepo.SaveAsync(vwModel.CollateralInformationModel);
                    }

                    if (vwModel.LoanParticularsInformationModel != null)
                    {
                        await _loanParticularsInformationRepo.SaveAsync(vwModel.LoanParticularsInformationModel, userId);
                    }

                    if (vwModel.SpouseModel != null && vwModel.SpouseModel.FirstName != null)
                    {
                        //vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        //vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

                        await _spouseRepo.SaveAsync(vwModel.SpouseModel);
                    }

                    if (vwModel.Form2PageModel != null)
                    {
                        await _form2PageRepo.SaveAsync(vwModel.Form2PageModel);
                    }
                }

                // last stage pass parameter code

                var applicantdata = await _applicantsPersonalInformationRepo.GetByUserAsync(user.Id);

                #region Notification

                var type = vwModel.ApplicantsPersonalInformationModel.Id == 0 ? "Added" : "Updated";
                var actiontype = type;

                var actionlink = $"Applicants/HLF068/{applicantdata.Code}";

                await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_APPLICANTSREQUESTS, actionlink, actiontype, applicantdata.Code, userId, companyId);

                #endregion Notification

                return Ok(applicantdata.Code);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[ModelStateValidations(typeof(UserModel))]
        public async Task<User> RegisterBenefeciary(UserModel user)
        {
            user.Position = "Beneficiary";

            // validate and  register user
            var userData = await _authService.RegisterUser(user);

            //save as benificiary
            await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

            return userData;
        }

        #region Helper Methods

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

        private static string GeneratePassword(string name)
        {
            name = name.Replace(" ", "");

            // Generate a GUID with 16 characters
            string guid = Guid.NewGuid().ToString("N").Substring(0, 16);

            // Concatenate GUID with name
            string combinedString = guid + name;

            // Use a random seed based on the current time
            Random rand = new Random();

            // Hash the combined string using SHA-256
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(combinedString);
                byte[] hash = sha256.ComputeHash(bytes);

                // Introduce additional randomness
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] ^= (byte)rand.Next(256);
                }

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                // Ensure a fixed length of 14 characters for the output password
                string hashedString = sb.ToString();
                string outputPassword = name + hashedString.Substring(0, 10);

                return outputPassword;
            }
        }

        private async Task<bool> UsernameExistsAsync(string username)
        {
            return (await _userRepo.GetAllAsync()).Any(x => x.UserName == username);
        }

        #endregion Helper Methods
    }
}