using AutoMapper;
using DevExpress.CodeParser;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
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
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using DMS.Web.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
        #region Fields

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
        private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBuyerConfirmationRepository _buyerConfirmationRepo;

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
            IRoleAccessRepository roleAccessRepo,
            IBeneficiaryInformationRepository beneficiaryInformationRepo,
            IWebHostEnvironment webHostEnvironment,
            IBuyerConfirmationRepository buyerConfirmationRepo)
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
            _beneficiaryInformationRepo = beneficiaryInformationRepo;
            _webHostEnvironment = webHostEnvironment;
            _buyerConfirmationRepo = buyerConfirmationRepo;
        }

        #endregion Fields

        #region Views

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

            ApplicantViewModel viewModel = new();

            ApplicantsPersonalInformationModel applicantInfoModel = new();

            var applicationRecord = await _applicantsPersonalInformationRepo.GetAllApplicationsByPagibigNumber(userData.PagibigNumber.ToString());

            if (applicationRecord.Count() > 0)
            {
                var latestRecord = applicationRecord.OrderByDescending(a => a.DateCreated).FirstOrDefault();

                //var latestRecord = applicationRecord.OrderByDescending(m => m.Code).ThenBy(m => m.DateModified).FirstOrDefault();

                //make it reverse
                if (latestRecord != null && ((int)latestRecord.ApprovalStatus == (int)AppStatusType.Deferred) || ((int)latestRecord.ApprovalStatus == (int)AppStatusType.Withdrawn))

                {
                    applicantInfoModel.isCanAppliedNewApplication = true;
                }
            }
            else
            { // if had no record yet means it can applied new application
                applicantInfoModel.isCanAppliedNewApplication = true;
            }

            applicantInfoModel.PagibigNumber = userData.PagibigNumber;
            applicantInfoModel.UserId = userData.Id;

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

                int userId = int.Parse(User.Identity.Name);
                var userInfo = await _userRepo.GetUserAsync(userId);

                if (applicantinfo == null)
                {
                    throw new Exception($"Transaction: {applicantCode}: no record Found!");
                }

                //if the application is not access by beneficiary
                if (applicantinfo.UserId != userId && userInfo.UserRoleId == (int)PredefinedRoleType.Beneficiary)
                {
                    return View("AccessDenied");
                }

                var barrowerInfo = await _barrowersInformationRepo.GetByApplicantIdAsync(applicantinfo.Id);

                ////if the application approvalStatus is not greater than 4 on pagibig viewer
                //if (applicantinfo.ApprovalStatus < (int)AppStatusType.DeveloperVerified && userInfo.UserRoleId == (int)PredefinedRoleType.Pagibig)
                //{
                //    return View("AccessDenied");
                //}

                var eligibilityPhaseDocument = await _documentVerificationRepo.GetByTypeAsync(1, applicantCode);
                var applicationPhaseDocument = await _documentVerificationRepo.GetByTypeAsync(2, applicantCode);

                var incompleteDocumentDataStage1 = eligibilityPhaseDocument.Where(dv => dv.TotalDocumentCount == 0).ToList();
                var incompleteDocumentDataStage2 = applicationPhaseDocument.Where(dv => dv.TotalDocumentCount == 0).ToList();

                applicantinfo.isRequiredDocumentsUploaded = false;

                //applicantinfo.StageNo = (applicantinfo.ApprovalStatus == (int)AppStatusType.PagibigVerified) ? 2 : 1;

                if ((applicantinfo.StageNo == 1 && incompleteDocumentDataStage1.Count > 0) || (applicantinfo.StageNo != 1 && incompleteDocumentDataStage2.Count > 0))
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
            try
            {
                var vwModel = new ApplicantViewModel();

                int userId = int.Parse(User.Identity.Name);

                var userInfo = await _userRepo.GetUserAsync(userId);

                string returnViewPage = "HLF068";

                bool hasBcf = false;

                if (userInfo.UserRoleId == (int)PredefinedRoleType.Beneficiary)
                {
                    var buyerConfirmationInfo = await _buyerConfirmationRepo.GetByUserAsync(userInfo.Id);

                    var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(userInfo.PagibigNumber);

                    hasBcf = beneficiaryData.IsBcfCreated;

                    if (buyerConfirmationInfo != null)
                    {
                        vwModel.BuyerConfirmationModel = buyerConfirmationInfo;

                        ////mostly not needed its on edit mode
                        //vwModel.BuyerConfirmationModel.HouseUnitModel = vwModel.BuyerConfirmationModel.HouseUnitModel ?? beneficiaryData.PropertyUnitLevelName;
                        //vwModel.BuyerConfirmationModel.ProjectProponentName = vwModel.BuyerConfirmationModel.ProjectProponentName ?? beneficiaryData.PropertyDeveloperName;

                        #region With Api Integration

                        //vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.PropertyDeveloperName;
                        //vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;
                        //vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

                        //vwModel.BuyerConfirmationModel.PropertyLocationId = beneficiaryData.PropertyLocationId;
                        //vwModel.BuyerConfirmationModel.PropertyDeveloperId = beneficiaryData.PropertyDeveloperId;
                        //vwModel.BuyerConfirmationModel.ProjectUnitId = beneficiaryData.PropertyUnitId;
                        //vwModel.BuyerConfirmationModel.PropertyProjectId = beneficiaryData.PropertyProjectId;

                        #endregion With Api Integration
                    }
                    else
                    {
                        vwModel.BuyerConfirmationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.LastName = beneficiaryData.LastName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.MobileNumber = beneficiaryData.MobileNumber;
                        vwModel.BuyerConfirmationModel.BirthDate = beneficiaryData.BirthDate;
                        vwModel.BuyerConfirmationModel.Email = beneficiaryData.Email;
                        vwModel.BuyerConfirmationModel.Suffix = userInfo.Suffix;

                        vwModel.BuyerConfirmationModel.PresentUnitName = beneficiaryData.PresentUnitName;
                        vwModel.BuyerConfirmationModel.PresentBuildingName = beneficiaryData.PresentBuildingName;
                        vwModel.BuyerConfirmationModel.PresentLotName = beneficiaryData.PresentLotName;
                        vwModel.BuyerConfirmationModel.PresentSubdivisionName = beneficiaryData.PresentSubdivisionName;
                        vwModel.BuyerConfirmationModel.PresentBaranggayName = beneficiaryData.PresentBaranggayName;
                        vwModel.BuyerConfirmationModel.PresentMunicipalityName = beneficiaryData.PresentMunicipalityName;
                        vwModel.BuyerConfirmationModel.PresentProvinceName = beneficiaryData.PresentProvinceName;
                        vwModel.BuyerConfirmationModel.PresentZipCode = beneficiaryData.PresentZipCode;
                        vwModel.BuyerConfirmationModel.PresentStreetName = beneficiaryData.PresentStreetName;
                        vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

                        //disable this if integrate the latest api
                        //vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.PropertyDeveloperName;
                        //vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.PropertyUnitLevelName;

                        #region With Api Integration

                        vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.DeveloperName;
                        vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;

                        vwModel.BuyerConfirmationModel.PropertyLocationId = beneficiaryData.PropertyLocationId;
                        vwModel.BuyerConfirmationModel.PropertyDeveloperId = beneficiaryData.PropertyDeveloperId;
                        vwModel.BuyerConfirmationModel.ProjectUnitId = beneficiaryData.PropertyUnitId;
                        vwModel.BuyerConfirmationModel.PropertyProjectId = beneficiaryData.PropertyProjectId;

                        #endregion With Api Integration
                    }

                    returnViewPage = "Beneficiary_HLF068";
                }

                //check if applicant code not null go to edit form
                if (applicantCode != null)
                {
                    var applicantinfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(applicantCode);

                    List<int> inActiveStatuses = new List<int> { 0, 2, 5, 9, 10, 11 };

                    if (applicantinfo == null)
                    {
                        throw new Exception($"Transaction ({applicantCode})" + " cant be accessible!");
                    }

                    //if current log user is beneficiary
                    if (userId != applicantinfo.UserId && userInfo.UserRoleId == (int)PredefinedRoleType.Beneficiary)
                    {
                        throw new Exception($"Transaction ({applicantCode})" + " cant be accessible!");
                    }
                    else if (userId == applicantinfo.UserId && userInfo.UserRoleId == (int)PredefinedRoleType.Beneficiary && !inActiveStatuses.Contains(applicantinfo.ApprovalStatus.Value))
                    {
                        throw new Exception($"Transaction ({applicantCode})" + " is currently in active status, cant be accessible!");
                    }

                    //var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(applicantinfo.PagibigNumber);

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
                        vwModel.BarrowersInformationModel.IsBcfCreated = hasBcf;
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

                    if (vwModel.BarrowersInformationModel.IsPresentAddressPermanentAddress)
                    {
                        vwModel.BarrowersInformationModel.PresentAddressIsPermanentAddress = true;
                    }
                }

                return View(returnViewPage, vwModel);
            }
            catch (Exception)
            {
                throw;
            }
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

            var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(pagibigNumber);

            var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);

            vwModel.ApplicantsPersonalInformationModel.UserId = userData.Id;

            vwModel.BarrowersInformationModel.FirstName = userData.FirstName ?? string.Empty;
            vwModel.BarrowersInformationModel.MiddleName = userData.MiddleName ?? string.Empty;
            vwModel.BarrowersInformationModel.LastName = userData.LastName ?? string.Empty;

            vwModel.BarrowersInformationModel.Email = userData.Email;

            vwModel.ApplicantsPersonalInformationModel.PagibigNumber = userData.PagibigNumber;
            vwModel.BarrowersInformationModel.Sex = userData.Gender;
            vwModel.BarrowersInformationModel.Suffix = userData.Suffix;

            if (beneficiaryData != null)
            {
                vwModel.BarrowersInformationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                vwModel.BarrowersInformationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                vwModel.BarrowersInformationModel.LastName = beneficiaryData.LastName ?? string.Empty;
                vwModel.BarrowersInformationModel.MobileNumber = beneficiaryData.MobileNumber;
                vwModel.BarrowersInformationModel.BirthDate = beneficiaryData.BirthDate;
                vwModel.BarrowersInformationModel.MobileNumber = beneficiaryData.MobileNumber;
                vwModel.BarrowersInformationModel.Sex = beneficiaryData.Sex;
                vwModel.BarrowersInformationModel.Email = beneficiaryData.Email;
                vwModel.BarrowersInformationModel.PresentUnitName = beneficiaryData.PresentUnitName;
                vwModel.BarrowersInformationModel.PresentBuildingName = beneficiaryData.PresentBuildingName;
                vwModel.BarrowersInformationModel.PresentLotName = beneficiaryData.PresentLotName;
                vwModel.BarrowersInformationModel.PresentSubdivisionName = beneficiaryData.PresentSubdivisionName;
                vwModel.BarrowersInformationModel.PresentBaranggayName = beneficiaryData.PresentBaranggayName;
                vwModel.BarrowersInformationModel.PresentMunicipalityName = beneficiaryData.PresentMunicipalityName;
                vwModel.BarrowersInformationModel.PresentProvinceName = beneficiaryData.PresentProvinceName;
                vwModel.BarrowersInformationModel.PresentZipCode = beneficiaryData.PresentZipCode;

                vwModel.BarrowersInformationModel.PermanentUnitName = beneficiaryData.PermanentUnitName;
                vwModel.BarrowersInformationModel.PermanentBuildingName = beneficiaryData.PermanentBuildingName;
                vwModel.BarrowersInformationModel.PermanentLotName = beneficiaryData.PermanentLotName;
                vwModel.BarrowersInformationModel.PermanentSubdivisionName = beneficiaryData.PermanentSubdivisionName;
                vwModel.BarrowersInformationModel.PermanentBaranggayName = beneficiaryData.PermanentBaranggayName;
                vwModel.BarrowersInformationModel.PermanentMunicipalityName = beneficiaryData.PermanentMunicipalityName;
                vwModel.BarrowersInformationModel.PermanentProvinceName = beneficiaryData.PermanentProvinceName;
                vwModel.BarrowersInformationModel.PermanentZipCode = beneficiaryData.PermanentZipCode;

                //Old
                vwModel.BarrowersInformationModel.PropertyDeveloperName = beneficiaryData.PropertyDeveloperName;
                vwModel.BarrowersInformationModel.PropertyLocation = beneficiaryData.PropertyLocation;
                vwModel.BarrowersInformationModel.PropertyUnitLevelName = beneficiaryData.PropertyUnitLevelName;

                //New
                //vwModel.BarrowersInformationModel.PropertyDeveloperName = beneficiaryData.DeveloperName;
                //vwModel.BarrowersInformationModel.PropertyLocation = beneficiaryData.LocationName;
                //vwModel.BarrowersInformationModel.PropertyUnitLevelName = beneficiaryData.HouseUnitDescription;

                //vwModel.BarrowersInformationModel.IsPermanentAddressAbroad = beneficiaryData.IsPermanentAddressAbroad.Value; // no condition because all address is required
                //vwModel.BarrowersInformationModel.IsPresentAddressAbroad = beneficiaryData.IsPresentAddressAbroad.Value; // no condition because all address is required
            }

            return View(vwModel);
        }

        [Route("[controller]/HousingLoanForm/{pagibigNumber?}")]
        public async Task<IActionResult> HousingLoanForm(string? pagibigNumber = null)
        {
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var vwModel = new ApplicantViewModel();

            var beneficiaryData = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(pagibigNumber);

            var activeapplication = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(beneficiaryData.UserId, companyId);

            List<int> activeStatus = new List<int> { 2, 5, 9, 10 };

            if (activeapplication != null && !activeStatus.Contains(activeapplication.ApprovalStatus.Value))
            {
                var applicantinfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(activeapplication.Code);

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

                    vwModel.BarrowersInformationModel.PropertyDeveloperName = beneficiaryData.DeveloperName;
                    vwModel.BarrowersInformationModel.PropertyLocation = beneficiaryData.LocationName;
                    vwModel.BarrowersInformationModel.PropertyUnitLevelName = beneficiaryData.HouseUnitDescription;

                    vwModel.BarrowersInformationModel.IsBcfCreated = beneficiaryData.IsBcfCreated;
                }
                if (vwModel.BarrowersInformationModel.IsPresentAddressPermanentAddress)
                {
                    vwModel.BarrowersInformationModel.PresentAddressIsPermanentAddress = true;
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

                var buyerConfirmationInfo = await _buyerConfirmationRepo.GetByUserAsync(beneficiaryData.UserId);

                if (buyerConfirmationInfo != null)
                {
                    vwModel.BuyerConfirmationModel = buyerConfirmationInfo;

                    //mostly not needed its on edit mode
                    // vwModel.BuyerConfirmationModel.HouseUnitModel = vwModel.BuyerConfirmationModel.HouseUnitModel ?? beneficiaryData.PropertyUnitLevelName;
                    //vwModel.BuyerConfirmationModel.ProjectProponentName = vwModel.BuyerConfirmationModel.ProjectProponentName?? beneficiaryData.PropertyDeveloperName;

                    #region With Api Integration

                    vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.DeveloperName;
                    vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;

                    //vwModel.BuyerConfirmationModel.PropertyLocationId = beneficiaryData.PropertyLocationId;
                    //vwModel.BuyerConfirmationModel.PropertyDeveloperId = beneficiaryData.PropertyDeveloperId;
                    //vwModel.BuyerConfirmationModel.ProjectUnitId = beneficiaryData.PropertyUnitId;
                    //vwModel.BuyerConfirmationModel.PropertyProjectId = beneficiaryData.PropertyProjectId;

                    #endregion With Api Integration
                }
                else
                {
                    var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);

                    vwModel.BuyerConfirmationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                    vwModel.BuyerConfirmationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                    vwModel.BuyerConfirmationModel.LastName = beneficiaryData.LastName ?? string.Empty;
                    vwModel.BuyerConfirmationModel.MobileNumber = beneficiaryData.MobileNumber;
                    vwModel.BuyerConfirmationModel.BirthDate = beneficiaryData.BirthDate;
                    vwModel.BuyerConfirmationModel.Email = beneficiaryData.Email;
                    vwModel.BuyerConfirmationModel.Suffix = userData.Suffix;

                    vwModel.BuyerConfirmationModel.PresentUnitName = beneficiaryData.PresentUnitName;
                    vwModel.BuyerConfirmationModel.PresentBuildingName = beneficiaryData.PresentBuildingName;
                    vwModel.BuyerConfirmationModel.PresentLotName = beneficiaryData.PresentLotName;
                    vwModel.BuyerConfirmationModel.PresentSubdivisionName = beneficiaryData.PresentSubdivisionName;
                    vwModel.BuyerConfirmationModel.PresentBaranggayName = beneficiaryData.PresentBaranggayName;
                    vwModel.BuyerConfirmationModel.PresentMunicipalityName = beneficiaryData.PresentMunicipalityName;
                    vwModel.BuyerConfirmationModel.PresentProvinceName = beneficiaryData.PresentProvinceName;
                    vwModel.BuyerConfirmationModel.PresentZipCode = beneficiaryData.PresentZipCode;
                    vwModel.BuyerConfirmationModel.PresentStreetName = beneficiaryData.PresentStreetName;
                    vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

                    vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.PropertyDeveloperName;
                    vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.PropertyUnitLevelName;

                    #region With Api Integration

                    //vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.DeveloperName;
                    //vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;

                    //vwModel.BuyerConfirmationModel.PropertyLocationId = beneficiaryData.PropertyLocationId;
                    //vwModel.BuyerConfirmationModel.PropertyDeveloperId = beneficiaryData.PropertyDeveloperId;
                    //vwModel.BuyerConfirmationModel.ProjectUnitId = beneficiaryData.PropertyUnitId;
                    //vwModel.BuyerConfirmationModel.PropertyProjectId = beneficiaryData.PropertyProjectId;

                    #endregion With Api Integration
                }

                return View("HousingLoanForm", vwModel);
            }
            else
            {
                if (beneficiaryData != null)
                {
                    var userData = await _userRepo.GetByPagibigNumberAsync(pagibigNumber);

                    vwModel.ApplicantsPersonalInformationModel.UserId = userData.Id;

                    vwModel.BarrowersInformationModel.FirstName = userData.FirstName ?? string.Empty;
                    vwModel.BarrowersInformationModel.MiddleName = userData.MiddleName ?? string.Empty;
                    vwModel.BarrowersInformationModel.LastName = userData.LastName ?? string.Empty;

                    vwModel.BarrowersInformationModel.Email = userData.Email;

                    vwModel.ApplicantsPersonalInformationModel.PagibigNumber = userData.PagibigNumber;
                    vwModel.BarrowersInformationModel.Sex = userData.Gender;
                    vwModel.BarrowersInformationModel.Suffix = userData.Suffix;

                    vwModel.BarrowersInformationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                    vwModel.BarrowersInformationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                    vwModel.BarrowersInformationModel.LastName = beneficiaryData.LastName ?? string.Empty;
                    vwModel.BarrowersInformationModel.MobileNumber = beneficiaryData.MobileNumber;
                    vwModel.BarrowersInformationModel.BirthDate = beneficiaryData.BirthDate;
                    vwModel.BarrowersInformationModel.Sex = beneficiaryData.Sex;
                    vwModel.BarrowersInformationModel.Email = beneficiaryData.Email;
                    vwModel.BarrowersInformationModel.PresentStreetName = beneficiaryData.PresentStreetName;
                    vwModel.BarrowersInformationModel.PresentUnitName = beneficiaryData.PresentUnitName;
                    vwModel.BarrowersInformationModel.PresentBuildingName = beneficiaryData.PresentBuildingName;
                    vwModel.BarrowersInformationModel.PresentLotName = beneficiaryData.PresentLotName;
                    vwModel.BarrowersInformationModel.PresentSubdivisionName = beneficiaryData.PresentSubdivisionName;
                    vwModel.BarrowersInformationModel.PresentBaranggayName = beneficiaryData.PresentBaranggayName;
                    vwModel.BarrowersInformationModel.PresentMunicipalityName = beneficiaryData.PresentMunicipalityName;
                    vwModel.BarrowersInformationModel.PresentProvinceName = beneficiaryData.PresentProvinceName;
                    vwModel.BarrowersInformationModel.PresentZipCode = beneficiaryData.PresentZipCode;

                    vwModel.BarrowersInformationModel.PermanentStreetName = beneficiaryData.PermanentStreetName;
                    vwModel.BarrowersInformationModel.PermanentUnitName = beneficiaryData.PermanentUnitName;
                    vwModel.BarrowersInformationModel.PermanentBuildingName = beneficiaryData.PermanentBuildingName;
                    vwModel.BarrowersInformationModel.PermanentLotName = beneficiaryData.PermanentLotName;
                    vwModel.BarrowersInformationModel.PermanentSubdivisionName = beneficiaryData.PermanentSubdivisionName;
                    vwModel.BarrowersInformationModel.PermanentBaranggayName = beneficiaryData.PermanentBaranggayName;
                    vwModel.BarrowersInformationModel.PermanentMunicipalityName = beneficiaryData.PermanentMunicipalityName;
                    vwModel.BarrowersInformationModel.PermanentProvinceName = beneficiaryData.PermanentProvinceName;
                    vwModel.BarrowersInformationModel.PermanentZipCode = beneficiaryData.PermanentZipCode;

                    ////Old
                    //vwModel.BarrowersInformationModel.PropertyDeveloperName = beneficiaryData.PropertyDeveloperName;
                    //vwModel.BarrowersInformationModel.PropertyLocation = beneficiaryData.PropertyLocation;
                    //vwModel.BarrowersInformationModel.PropertyUnitLevelName = beneficiaryData.PropertyUnitLevelName;

                    //New
                    vwModel.BarrowersInformationModel.PropertyDeveloperName = beneficiaryData.DeveloperName;
                    vwModel.BarrowersInformationModel.PropertyLocation = beneficiaryData.LocationName;
                    vwModel.BarrowersInformationModel.PropertyUnitLevelName = beneficiaryData.HouseUnitDescription;

                    vwModel.BarrowersInformationModel.IsBcfCreated = beneficiaryData.IsBcfCreated;

                    var buyerConfirmationInfo = await _buyerConfirmationRepo.GetByUserAsync(beneficiaryData.UserId);

                    if (buyerConfirmationInfo != null)
                    {
                        vwModel.BuyerConfirmationModel = buyerConfirmationInfo;
                        vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.DeveloperName;
                        vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;
                    }
                    else
                    {
                        vwModel.BuyerConfirmationModel.FirstName = beneficiaryData.FirstName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.MiddleName = beneficiaryData.MiddleName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.LastName = beneficiaryData.LastName ?? string.Empty;
                        vwModel.BuyerConfirmationModel.MobileNumber = beneficiaryData.MobileNumber;
                        vwModel.BuyerConfirmationModel.BirthDate = beneficiaryData.BirthDate;
                        vwModel.BuyerConfirmationModel.Email = beneficiaryData.Email;
                        vwModel.BuyerConfirmationModel.Suffix = userData.Suffix;

                        vwModel.BuyerConfirmationModel.PresentUnitName = beneficiaryData.PresentUnitName;
                        vwModel.BuyerConfirmationModel.PresentBuildingName = beneficiaryData.PresentBuildingName;
                        vwModel.BuyerConfirmationModel.PresentLotName = beneficiaryData.PresentLotName;
                        vwModel.BuyerConfirmationModel.PresentSubdivisionName = beneficiaryData.PresentSubdivisionName;
                        vwModel.BuyerConfirmationModel.PresentBaranggayName = beneficiaryData.PresentBaranggayName;
                        vwModel.BuyerConfirmationModel.PresentMunicipalityName = beneficiaryData.PresentMunicipalityName;
                        vwModel.BuyerConfirmationModel.PresentProvinceName = beneficiaryData.PresentProvinceName;
                        vwModel.BuyerConfirmationModel.PresentZipCode = beneficiaryData.PresentZipCode;
                        vwModel.BuyerConfirmationModel.PresentStreetName = beneficiaryData.PresentStreetName;
                        vwModel.BuyerConfirmationModel.PagibigNumber = beneficiaryData.PagibigNumber;

                        //hide this if using update api integration
                        //vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.PropertyDeveloperName;
                        //vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.PropertyUnitLevelName;

                        #region With Api Integration

                        vwModel.BuyerConfirmationModel.ProjectProponentName = beneficiaryData.DeveloperName;
                        vwModel.BuyerConfirmationModel.HouseUnitModel = beneficiaryData.HouseUnitDescription;

                        vwModel.BuyerConfirmationModel.PropertyLocationId = beneficiaryData.PropertyLocationId;
                        vwModel.BuyerConfirmationModel.PropertyDeveloperId = beneficiaryData.PropertyDeveloperId;
                        vwModel.BuyerConfirmationModel.ProjectUnitId = beneficiaryData.PropertyUnitId;
                        vwModel.BuyerConfirmationModel.PropertyProjectId = beneficiaryData.PropertyProjectId;

                        #endregion With Api Integration
                    }

                    //vwModel.BarrowersInformationModel.IsPermanentAddressAbroad = beneficiaryData.IsPermanentAddressAbroad.Value; // no condition because all address is required
                    //vwModel.BarrowersInformationModel.IsPresentAddressAbroad = beneficiaryData.IsPresentAddressAbroad.Value; // no condition because all address is required
                }

                return View("Beneficiary_HLF068", vwModel);
            }
        }

        #endregion Views

        #region API Getters

        public async Task<IActionResult> GetUsersByRoleName(string roleName)
        {
            try
            {
                int companyId = int.Parse(User.FindFirstValue("Company"));

                var result = await _userRepo.spGetByRoleName(roleName, companyId);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> GetPurposeOfLoan() =>
            Ok(await _purposeOfLoanRepo.GetAllAsync());

        public async Task<IActionResult> GetModeOfPayment() =>
            Ok(await _modeOfPaymentRepo.GetAllAsync());

        public async Task<IActionResult> GetPropertyType() =>
            Ok(await _propertyTypeRepo.GetAllAsync());

        public async Task<IActionResult> GetApplicants()

        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var data = await _applicantsPersonalInformationRepo.GetApplicantsAsync(roleId, companyId);

            return Ok(data);
        }

        public async Task<IActionResult> GetMyApplications()
        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var applicationData = await _applicantsPersonalInformationRepo.GetApplicantsAsync(roleId, companyId);

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
            int companyId = int.Parse(User.FindFirstValue("Company"));
            //int userId = int.Parse(User.Identity.Name);

            return Ok(await _applicantsPersonalInformationRepo.GetApprovalTotalInfo(null, companyId));
        }

        public async Task<IActionResult> GetBeneficiaryApprovalTotalInfo()
        {
            int companyId = int.Parse(User.FindFirstValue("Company"));
            int userId = int.Parse(User.Identity.Name);

            return Ok(await _applicantsPersonalInformationRepo.GetApprovalTotalInfo(userId, companyId));
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

        [Route("[controller]/GetAllApplicationByPagibigNum/{pagibigNumber}")]
        public async Task<IActionResult> GetAllApplicationByPagibigNum(string pagibigNumber)
        {
            var result = await _applicantsPersonalInformationRepo.GetAllApplicationsByPagibigNumber(pagibigNumber);
            return Ok(result);
        }

        public async Task<IActionResult> GetTotalApplicants()
        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var result = await _applicantsPersonalInformationRepo.GetTotalApplication(roleId, companyId);
            return Ok(result);
        }

        public async Task<IActionResult> GetTotalCreditVerif()
        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var result = await _applicantsPersonalInformationRepo.GetTotalCreditVerif(companyId);
            return Ok(result);
        }

        public async Task<IActionResult> GetTotalAppVerif()
        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            var userdata = await _userRepo.GetUserAsync(userId);
            int roleId = userdata.UserRoleId.Value;

            var result = await _applicantsPersonalInformationRepo.GetTotalAppVerif(companyId);
            return Ok(result);
        }

        public async Task<IActionResult> GetBeneficiaryActiveApplication()
        {
            int userId = int.Parse(User.Identity.Name);
            int companyId = int.Parse(User.FindFirstValue("Company"));

            dynamic applicantInfo;
            var data = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(userId, companyId);

            if (data is null)
            {
                applicantInfo = new ApplicantsPersonalInformationModel()
                {
                    ApplicationStatus = null,
                    Code = "------",
                    LoanAmount = 0,
                    LoanYears = 0,
                    ProjectLocation = "----------"
                };
            }
            else
            {
                applicantInfo = await _applicantsPersonalInformationRepo.GetByCodeAsync(data.Code);
            }

            return Ok(applicantInfo);
        }

        public async Task<IActionResult> GetTimelineStatus()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));

                var application = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(userId, companyId);

                var timeline = await _applicantsPersonalInformationRepo.GetApplicationTimelineByCode(application?.Code, companyId);

                return Ok(timeline);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion API Getters

        #region API Operations

        public async Task<IActionResult> CheckBcf()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                var bcfData = await _buyerConfirmationRepo.GetByUserAsync(userId);

                return Ok(bcfData is not null);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> CheckCurrentHlaf()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));

                var data = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(userId, companyId);

                List<int> inactiveStatus = new() { 2, 5, 9, 10 };

                bool flag = data is not null && !inactiveStatus.Contains(data.ApprovalStatus.Value);

                return Ok(flag);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBcfFlag(bool flag)
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                var currentUser = await _userRepo.GetUserAsync(userId);
                var bnfInfo = await _beneficiaryInformationRepo.GetByPagibigNumberAsync(currentUser.PagibigNumber);

                bnfInfo.IsBcfCreated = flag;

                await _beneficiaryInformationRepo.SaveAsync(bnfInfo, userId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveHLF068(ApplicantViewModel vwModel)
        {
            try
            {
                // Manually remove validation errors for properties of BuyerConfirmationModel
                var buyerConfirmationProperties = typeof(BuyerConfirmationModel).GetProperties()
                    .SelectMany(prop => ModelState.Keys.Where(key => key.StartsWith($"BuyerConfirmationModel.{prop.Name}")));

                foreach (var propertyKey in buyerConfirmationProperties)
                {
                    ModelState.Remove(propertyKey);
                }

                if (!ModelState.IsValid)
                {
                    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                }

                var user = new User();
                var beneficiaryModel = new BeneficiaryInformationModel();
                var barrowerModel = new BarrowersInformation();
                int userId = int.Parse(User.Identity.Name);

                var userinfo = await _userRepo.GetUserAsync(userId);
                var currentuserRoleId = userinfo.UserRoleId;

                //current user is beneficiary

                int companyId = int.Parse(User.FindFirstValue("Company"));
                var applicantCode = string.Empty;

                //Unmasked
                vwModel.ApplicantsPersonalInformationModel.PagibigNumber = vwModel.ApplicantsPersonalInformationModel.PagibigNumber.Replace("-", "") ?? string.Empty;

                //create  new beneficiary and housingloan application

                if (vwModel.ApplicantsPersonalInformationModel.Id == 0)
                {
                    //current user is beneficiary
                    if (currentuserRoleId == (int)PredefinedRoleType.Beneficiary)
                    {
                        var applicationDetail = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(userId, companyId);

                        if (applicationDetail != null)
                        {
                            if (applicationDetail.ApprovalStatus != (int)AppStatusType.Deferred && applicationDetail.ApprovalStatus != (int)AppStatusType.Withdrawn && applicationDetail.ApprovalStatus != (int)AppStatusType.Disqualified && applicationDetail.ApprovalStatus != (int)AppStatusType.Discontinued)
                            {
                                return BadRequest("Can't be processed. You have a pending application!");
                            }
                        }
                    }

                    vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;

                    #region Register User and Send Email

                    if (vwModel.ApplicantsPersonalInformationModel.UserId == 0)
                    {
                        UserModel userModel = new()
                        {
                            Email = vwModel.BarrowersInformationModel.Email,
                            //Password = GeneratePassword(vwModel.BarrowersInformationModel.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                            Password = GenerateRandomPassword(), //sample output aDf!23@4kLp
                            UserName = await GenerateTemporaryUsernameAsync(),
                            FirstName = vwModel.BarrowersInformationModel.FirstName,
                            LastName = vwModel.BarrowersInformationModel.LastName,
                            MiddleName = vwModel.BarrowersInformationModel.MiddleName,
                            Gender = vwModel.BarrowersInformationModel.Sex,
                            PagibigNumber = vwModel.ApplicantsPersonalInformationModel.PagibigNumber,
                            CompanyId = companyId
                        };

                        //save beneficiary user
                        user = await RegisterBenefeciary(userModel);

                        //// reverse map parameter need for email sending
                        //var userdata = _mapper.Map<UserModel>(user);

                        // make the usage of hangfire
                        userModel.Action = "created";
                        _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential2(userModel, _webHostEnvironment.WebRootPath));

                        #region Create BeneficiaryInformation

                        beneficiaryModel.UserId = user.Id;
                        beneficiaryModel.CompanyId = 1;
                        beneficiaryModel.PagibigNumber = user.PagibigNumber;
                        beneficiaryModel.LastName = vwModel.BarrowersInformationModel.LastName;
                        beneficiaryModel.FirstName = vwModel.BarrowersInformationModel.FirstName;
                        beneficiaryModel.MiddleName = vwModel.BarrowersInformationModel.MiddleName;
                        beneficiaryModel.MobileNumber = vwModel.BarrowersInformationModel.MobileNumber;
                        beneficiaryModel.BirthDate = vwModel.BarrowersInformationModel.BirthDate;
                        beneficiaryModel.MobileNumber = vwModel.BarrowersInformationModel.MobileNumber;
                        beneficiaryModel.Sex = vwModel.BarrowersInformationModel.Sex;
                        beneficiaryModel.Email = vwModel.BarrowersInformationModel.Email;

                        beneficiaryModel.PropertyDeveloperName = vwModel.BarrowersInformationModel.PropertyDeveloperName;

                        beneficiaryModel.PropertyUnitLevelName = vwModel.BarrowersInformationModel.PropertyUnitLevelName;

                        beneficiaryModel.PropertyLocation = vwModel.BarrowersInformationModel.PropertyLocation;

                        beneficiaryModel.PermanentUnitName = vwModel.BarrowersInformationModel.PermanentUnitName;
                        beneficiaryModel.PermanentBuildingName = vwModel.BarrowersInformationModel.PermanentBuildingName;
                        beneficiaryModel.PermanentStreetName = vwModel.BarrowersInformationModel.PermanentStreetName;
                        beneficiaryModel.PermanentLotName = vwModel.BarrowersInformationModel.PermanentLotName;
                        beneficiaryModel.PermanentSubdivisionName = vwModel.BarrowersInformationModel.PermanentSubdivisionName;
                        beneficiaryModel.PermanentBaranggayName = vwModel.BarrowersInformationModel.PermanentBaranggayName;
                        beneficiaryModel.PermanentMunicipalityName = vwModel.BarrowersInformationModel.PermanentMunicipalityName;
                        beneficiaryModel.PermanentProvinceName = vwModel.BarrowersInformationModel.PermanentProvinceName;
                        beneficiaryModel.PermanentZipCode = vwModel.BarrowersInformationModel.PermanentZipCode;

                        if (vwModel.BarrowersInformationModel.PresentAddressIsPermanentAddress)
                        {
                            beneficiaryModel.PresentStreetName = vwModel.BarrowersInformationModel.PresentStreetName;
                            beneficiaryModel.PresentUnitName = vwModel.BarrowersInformationModel.PermanentUnitName;
                            beneficiaryModel.PresentBuildingName = vwModel.BarrowersInformationModel.PermanentBuildingName;
                            beneficiaryModel.PresentLotName = vwModel.BarrowersInformationModel.PermanentLotName;
                            beneficiaryModel.PresentSubdivisionName = vwModel.BarrowersInformationModel.PermanentSubdivisionName;
                            beneficiaryModel.PresentBaranggayName = vwModel.BarrowersInformationModel.PermanentBaranggayName;
                            beneficiaryModel.PresentMunicipalityName = vwModel.BarrowersInformationModel.PermanentMunicipalityName;
                            beneficiaryModel.PresentProvinceName = vwModel.BarrowersInformationModel.PermanentProvinceName;
                            beneficiaryModel.PresentZipCode = vwModel.BarrowersInformationModel.PermanentZipCode;
                        }
                        else
                        {
                            beneficiaryModel.PresentStreetName = vwModel.BarrowersInformationModel.PresentStreetName;
                            beneficiaryModel.PresentUnitName = vwModel.BarrowersInformationModel.PresentUnitName;
                            beneficiaryModel.PresentBuildingName = vwModel.BarrowersInformationModel.PresentBuildingName;
                            beneficiaryModel.PresentLotName = vwModel.BarrowersInformationModel.PresentLotName;
                            beneficiaryModel.PresentSubdivisionName = vwModel.BarrowersInformationModel.PresentSubdivisionName;
                            beneficiaryModel.PresentBaranggayName = vwModel.BarrowersInformationModel.PresentBaranggayName;
                            beneficiaryModel.PresentMunicipalityName = vwModel.BarrowersInformationModel.PresentMunicipalityName;
                            beneficiaryModel.PresentProvinceName = vwModel.BarrowersInformationModel.PresentProvinceName;
                            beneficiaryModel.PresentZipCode = vwModel.BarrowersInformationModel.PresentZipCode;
                        }

                        beneficiaryModel.PropertyDeveloperName = vwModel.BarrowersInformationModel.PropertyDeveloperName;
                        beneficiaryModel.PropertyLocation = vwModel.BarrowersInformationModel.PropertyLocation;
                        beneficiaryModel.PropertyUnitLevelName = vwModel.BarrowersInformationModel.PropertyUnitLevelName;

                        beneficiaryModel.IsPermanentAddressAbroad = vwModel.BarrowersInformationModel.IsPermanentAddressAbroad;  // no condition because all address is required
                        beneficiaryModel.IsPresentAddressAbroad = vwModel.BarrowersInformationModel.IsPresentAddressAbroad; // no condition because all address is required

                        await _beneficiaryInformationRepo.SaveAsync(beneficiaryModel, userId);

                        #endregion Create BeneficiaryInformation

                        vwModel.ApplicantsPersonalInformationModel.ApprovalStatus = vwModel.ApplicantsPersonalInformationModel.EncodedStatus;
                    }
                    else
                    {
                        vwModel.ApplicantsPersonalInformationModel.ApprovalStatus = vwModel.ApplicantsPersonalInformationModel.EncodedStatus;

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

                    vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;

                    var newApplicantData = await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    applicantCode = newApplicantData.Code;

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

                        vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

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

                    var applicationData = await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    applicantCode = applicationData.Code;

                    user.Id = vwModel.ApplicantsPersonalInformationModel.UserId;

                    if (vwModel.BarrowersInformationModel != null)
                    {
                        var barrowerData = await _barrowersInformationRepo.SaveAsync(vwModel.BarrowersInformationModel);

                        #region Create BeneficiaryInformation

                        //beneficiaryModel.UserId = user.Id;
                        //beneficiaryModel.CompanyId = 1;
                        //beneficiaryModel.PagibigNumber = user.PagibigNumber;
                        //beneficiaryModel.LastName = barrowerData.LastName;
                        //beneficiaryModel.FirstName = barrowerData.FirstName;
                        //beneficiaryModel.MiddleName = barrowerData.MiddleName;
                        //beneficiaryModel.MobileNumber = barrowerData.MobileNumber;
                        //beneficiaryModel.BirthDate = barrowerData.BirthDate;
                        //beneficiaryModel.Sex = barrowerData.Sex;
                        //beneficiaryModel.Email = barrowerData.Email;
                        //beneficiaryModel.PresentUnitName = barrowerData.PresentUnitName;
                        //beneficiaryModel.PresentBuildingName = barrowerData.PresentBuildingName;
                        //beneficiaryModel.PresentLotName = barrowerData.PresentLotName;
                        //beneficiaryModel.PresentSubdivisionName = barrowerData.PresentSubdivisionName;
                        //beneficiaryModel.PresentBaranggayName = barrowerData.PresentBaranggayName;
                        //beneficiaryModel.PresentMunicipalityName = barrowerData.PresentMunicipalityName;
                        //beneficiaryModel.PresentProvinceName = barrowerData.PresentProvinceName;
                        //beneficiaryModel.PresentZipCode = barrowerData.PresentZipCode;

                        //beneficiaryModel.PermanentUnitName = barrowerData.PermanentUnitName;
                        //beneficiaryModel.PermanentBuildingName = barrowerData.PermanentBuildingName;
                        //beneficiaryModel.PermanentLotName = barrowerData.PermanentLotName;
                        //beneficiaryModel.PermanentSubdivisionName = barrowerData.PermanentSubdivisionName;
                        //beneficiaryModel.PermanentBaranggayName = barrowerData.PermanentBaranggayName;
                        //beneficiaryModel.PermanentMunicipalityName = barrowerData.PermanentMunicipalityName;
                        //beneficiaryModel.PermanentProvinceName = barrowerData.PermanentProvinceName;
                        //beneficiaryModel.PermanentZipCode = barrowerData.PermanentZipCode;

                        //beneficiaryModel.PropertyDeveloperName = barrowerData.PropertyDeveloperName;
                        //beneficiaryModel.PropertyLocation = barrowerData.PropertyLocation;
                        //beneficiaryModel.PropertyUnitLevelName = barrowerData.PropertyUnitLevelName;

                        //beneficiaryModel.IsPermanentAddressAbroad = barrowerData.IsPermanentAddressAbroad.Value;  // no condition because all address is required
                        //beneficiaryModel.IsPresentAddressAbroad = barrowerData.IsPresentAddressAbroad.Value; // no condition because all address is required

                        //await _beneficiaryInformationRepo.SaveAsync(beneficiaryModel, 1);

                        #endregion Create BeneficiaryInformation
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
                        vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

                        await _spouseRepo.SaveAsync(vwModel.SpouseModel);
                    }

                    if (vwModel.Form2PageModel != null)
                    {
                        await _form2PageRepo.SaveAsync(vwModel.Form2PageModel);
                    }
                }

                // last stage pass parameter code

                #region Notification

                var type = vwModel.ApplicantsPersonalInformationModel.Id == 0 ? "Added" : "Updated";
                var actiontype = type;

                var actionlink = $"Applicants/Details/{applicantCode}";

                await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_APPLICANTSREQUESTS, actionlink, actiontype, applicantCode, userId, companyId);

                #endregion Notification

                return Ok(applicantCode);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveBCFHLAF(ApplicantViewModel vwModel)
        {
            try
            {
                if (vwModel.BuyerConfirmationModel.ApprovalStatus is (int)AppStatusType.DeveloperVerified)
                {
                    // Manually remove validation errors for properties of BuyerConfirmationModel
                    var buyerConfirmationProperties = typeof(BuyerConfirmationModel).GetProperties()
                        .SelectMany(prop => ModelState.Keys.Where(key => key.StartsWith($"BuyerConfirmationModel.{prop.Name}")));

                    foreach (var propertyKey in buyerConfirmationProperties)
                    {
                        ModelState.Remove(propertyKey);
                    }
                }

                if (!ModelState.IsValid)
                {
                    return Conflict(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));
                }

                var user = new User();
                var beneficiaryModel = new BeneficiaryInformationModel();
                var barrowerModel = new BarrowersInformation();
                int userId = int.Parse(User.Identity.Name);

                var userinfo = await _userRepo.GetUserAsync(userId);
                var currentuserRoleId = userinfo.UserRoleId;

                //current user is beneficiary

                int companyId = int.Parse(User.FindFirstValue("Company"));
                var applicantCode = string.Empty;

                //Unmasked
                vwModel.ApplicantsPersonalInformationModel.PagibigNumber = vwModel.ApplicantsPersonalInformationModel.PagibigNumber.Replace("-", "") ?? string.Empty;
                vwModel.ApplicantsPersonalInformationModel.CompanyId = companyId;
                vwModel.BuyerConfirmationModel.CompanyId = companyId;

                //create new beneficiary and housingloan application
                vwModel.BuyerConfirmationModel.UserId ??= userId;

                if (vwModel.BuyerConfirmationModel.ApprovalStatus is (int)AppStatusType.ForResubmition)
                {
                    vwModel.BuyerConfirmationModel.ApprovalStatus = (int)AppStatusType.Draft;
                }

                if (vwModel.BuyerConfirmationModel.ApprovalStatus is not (int)AppStatusType.DeveloperVerified)
                {
                    await _buyerConfirmationRepo.SaveAsync(vwModel.BuyerConfirmationModel, userId);
                }

                if (vwModel.ApplicantsPersonalInformationModel.Id == 0)
                {
                    //current user is beneficiary
                    if (currentuserRoleId == (int)PredefinedRoleType.Beneficiary)
                    {
                        var applicationDetail = await _applicantsPersonalInformationRepo.GetCurrentApplicationByUser(userId, companyId);

                        if (applicationDetail != null)
                        {
                            if (applicationDetail.ApprovalStatus != (int)AppStatusType.Deferred && applicationDetail.ApprovalStatus != (int)AppStatusType.Withdrawn && applicationDetail.ApprovalStatus != (int)AppStatusType.Disqualified && applicationDetail.ApprovalStatus != (int)AppStatusType.Discontinued)
                            {
                                return BadRequest("Can't be processed. You have a pending application!");
                            }
                        }
                    }

                    #region Register User and Send Email

                    if (vwModel.ApplicantsPersonalInformationModel.UserId == 0)
                    {
                        UserModel userModel = new()
                        {
                            Email = vwModel.BarrowersInformationModel.Email,
                            //Password = GeneratePassword(vwModel.BarrowersInformationModel.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                            Password = GenerateRandomPassword(), //sample output aDf!23@4kLp
                            UserName = await GenerateTemporaryUsernameAsync(),
                            FirstName = vwModel.BarrowersInformationModel.FirstName,
                            LastName = vwModel.BarrowersInformationModel.LastName,
                            MiddleName = vwModel.BarrowersInformationModel.MiddleName,
                            Gender = vwModel.BarrowersInformationModel.Sex,
                            PagibigNumber = vwModel.ApplicantsPersonalInformationModel.PagibigNumber,
                            CompanyId = companyId
                        };

                        //save beneficiary user
                        user = await RegisterBenefeciary(userModel);

                        //// reverse map parameter need for email sending
                        //var userdata = _mapper.Map<UserModel>(user);

                        // make the usage of hangfire
                        userModel.Action = "created";
                        _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential2(userModel, _webHostEnvironment.WebRootPath));

                        #region Create BeneficiaryInformation

                        beneficiaryModel.UserId = user.Id;
                        beneficiaryModel.CompanyId = 1;
                        beneficiaryModel.PagibigNumber = user.PagibigNumber;
                        beneficiaryModel.LastName = vwModel.BarrowersInformationModel.LastName;
                        beneficiaryModel.FirstName = vwModel.BarrowersInformationModel.FirstName;
                        beneficiaryModel.MiddleName = vwModel.BarrowersInformationModel.MiddleName;
                        beneficiaryModel.MobileNumber = vwModel.BarrowersInformationModel.MobileNumber;
                        beneficiaryModel.BirthDate = vwModel.BarrowersInformationModel.BirthDate;
                        beneficiaryModel.MobileNumber = vwModel.BarrowersInformationModel.MobileNumber;
                        beneficiaryModel.Sex = vwModel.BarrowersInformationModel.Sex;
                        beneficiaryModel.Email = vwModel.BarrowersInformationModel.Email;

                        beneficiaryModel.PropertyDeveloperName = vwModel.BarrowersInformationModel.PropertyDeveloperName;

                        beneficiaryModel.PropertyUnitLevelName = vwModel.BarrowersInformationModel.PropertyUnitLevelName;

                        beneficiaryModel.PropertyLocation = vwModel.BarrowersInformationModel.PropertyLocation;

                        beneficiaryModel.PermanentUnitName = vwModel.BarrowersInformationModel.PermanentUnitName;
                        beneficiaryModel.PermanentBuildingName = vwModel.BarrowersInformationModel.PermanentBuildingName;
                        beneficiaryModel.PermanentStreetName = vwModel.BarrowersInformationModel.PermanentStreetName;
                        beneficiaryModel.PermanentLotName = vwModel.BarrowersInformationModel.PermanentLotName;
                        beneficiaryModel.PermanentSubdivisionName = vwModel.BarrowersInformationModel.PermanentSubdivisionName;
                        beneficiaryModel.PermanentBaranggayName = vwModel.BarrowersInformationModel.PermanentBaranggayName;
                        beneficiaryModel.PermanentMunicipalityName = vwModel.BarrowersInformationModel.PermanentMunicipalityName;
                        beneficiaryModel.PermanentProvinceName = vwModel.BarrowersInformationModel.PermanentProvinceName;
                        beneficiaryModel.PermanentZipCode = vwModel.BarrowersInformationModel.PermanentZipCode;

                        if (vwModel.BarrowersInformationModel.PresentAddressIsPermanentAddress)
                        {
                            beneficiaryModel.PresentStreetName = vwModel.BarrowersInformationModel.PresentStreetName;
                            beneficiaryModel.PresentUnitName = vwModel.BarrowersInformationModel.PermanentUnitName;
                            beneficiaryModel.PresentBuildingName = vwModel.BarrowersInformationModel.PermanentBuildingName;
                            beneficiaryModel.PresentLotName = vwModel.BarrowersInformationModel.PermanentLotName;
                            beneficiaryModel.PresentSubdivisionName = vwModel.BarrowersInformationModel.PermanentSubdivisionName;
                            beneficiaryModel.PresentBaranggayName = vwModel.BarrowersInformationModel.PermanentBaranggayName;
                            beneficiaryModel.PresentMunicipalityName = vwModel.BarrowersInformationModel.PermanentMunicipalityName;
                            beneficiaryModel.PresentProvinceName = vwModel.BarrowersInformationModel.PermanentProvinceName;
                            beneficiaryModel.PresentZipCode = vwModel.BarrowersInformationModel.PermanentZipCode;
                        }
                        else
                        {
                            beneficiaryModel.PresentStreetName = vwModel.BarrowersInformationModel.PresentStreetName;
                            beneficiaryModel.PresentUnitName = vwModel.BarrowersInformationModel.PresentUnitName;
                            beneficiaryModel.PresentBuildingName = vwModel.BarrowersInformationModel.PresentBuildingName;
                            beneficiaryModel.PresentLotName = vwModel.BarrowersInformationModel.PresentLotName;
                            beneficiaryModel.PresentSubdivisionName = vwModel.BarrowersInformationModel.PresentSubdivisionName;
                            beneficiaryModel.PresentBaranggayName = vwModel.BarrowersInformationModel.PresentBaranggayName;
                            beneficiaryModel.PresentMunicipalityName = vwModel.BarrowersInformationModel.PresentMunicipalityName;
                            beneficiaryModel.PresentProvinceName = vwModel.BarrowersInformationModel.PresentProvinceName;
                            beneficiaryModel.PresentZipCode = vwModel.BarrowersInformationModel.PresentZipCode;
                        }

                        beneficiaryModel.PropertyDeveloperName = vwModel.BarrowersInformationModel.PropertyDeveloperName;
                        beneficiaryModel.PropertyLocation = vwModel.BarrowersInformationModel.PropertyLocation;
                        beneficiaryModel.PropertyUnitLevelName = vwModel.BarrowersInformationModel.PropertyUnitLevelName;

                        beneficiaryModel.IsPermanentAddressAbroad = vwModel.BarrowersInformationModel.IsPermanentAddressAbroad;  // no condition because all address is required
                        beneficiaryModel.IsPresentAddressAbroad = vwModel.BarrowersInformationModel.IsPresentAddressAbroad; // no condition because all address is required

                        await _beneficiaryInformationRepo.SaveAsync(beneficiaryModel, userId);

                        #endregion Create BeneficiaryInformation

                        vwModel.ApplicantsPersonalInformationModel.ApprovalStatus = vwModel.ApplicantsPersonalInformationModel.EncodedStatus;
                    }
                    else
                    {
                        vwModel.ApplicantsPersonalInformationModel.ApprovalStatus = vwModel.ApplicantsPersonalInformationModel.EncodedStatus;

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

                    var newApplicantData = await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    applicantCode = newApplicantData.Code;

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

                        vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

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
                    var applicationData = await _applicantsPersonalInformationRepo.SaveAsync(vwModel.ApplicantsPersonalInformationModel, userId);

                    applicantCode = applicationData.Code;

                    user.Id = vwModel.ApplicantsPersonalInformationModel.UserId;

                    if (vwModel.BarrowersInformationModel != null)
                    {
                        var barrowerData = await _barrowersInformationRepo.SaveAsync(vwModel.BarrowersInformationModel);

                        #region Create BeneficiaryInformation

                        //beneficiaryModel.UserId = user.Id;
                        //beneficiaryModel.CompanyId = 1;
                        //beneficiaryModel.PagibigNumber = user.PagibigNumber;
                        //beneficiaryModel.LastName = barrowerData.LastName;
                        //beneficiaryModel.FirstName = barrowerData.FirstName;
                        //beneficiaryModel.MiddleName = barrowerData.MiddleName;
                        //beneficiaryModel.MobileNumber = barrowerData.MobileNumber;
                        //beneficiaryModel.BirthDate = barrowerData.BirthDate;
                        //beneficiaryModel.Sex = barrowerData.Sex;
                        //beneficiaryModel.Email = barrowerData.Email;
                        //beneficiaryModel.PresentUnitName = barrowerData.PresentUnitName;
                        //beneficiaryModel.PresentBuildingName = barrowerData.PresentBuildingName;
                        //beneficiaryModel.PresentLotName = barrowerData.PresentLotName;
                        //beneficiaryModel.PresentSubdivisionName = barrowerData.PresentSubdivisionName;
                        //beneficiaryModel.PresentBaranggayName = barrowerData.PresentBaranggayName;
                        //beneficiaryModel.PresentMunicipalityName = barrowerData.PresentMunicipalityName;
                        //beneficiaryModel.PresentProvinceName = barrowerData.PresentProvinceName;
                        //beneficiaryModel.PresentZipCode = barrowerData.PresentZipCode;

                        //beneficiaryModel.PermanentUnitName = barrowerData.PermanentUnitName;
                        //beneficiaryModel.PermanentBuildingName = barrowerData.PermanentBuildingName;
                        //beneficiaryModel.PermanentLotName = barrowerData.PermanentLotName;
                        //beneficiaryModel.PermanentSubdivisionName = barrowerData.PermanentSubdivisionName;
                        //beneficiaryModel.PermanentBaranggayName = barrowerData.PermanentBaranggayName;
                        //beneficiaryModel.PermanentMunicipalityName = barrowerData.PermanentMunicipalityName;
                        //beneficiaryModel.PermanentProvinceName = barrowerData.PermanentProvinceName;
                        //beneficiaryModel.PermanentZipCode = barrowerData.PermanentZipCode;

                        //beneficiaryModel.PropertyDeveloperName = barrowerData.PropertyDeveloperName;
                        //beneficiaryModel.PropertyLocation = barrowerData.PropertyLocation;
                        //beneficiaryModel.PropertyUnitLevelName = barrowerData.PropertyUnitLevelName;

                        //beneficiaryModel.IsPermanentAddressAbroad = barrowerData.IsPermanentAddressAbroad.Value;  // no condition because all address is required
                        //beneficiaryModel.IsPresentAddressAbroad = barrowerData.IsPresentAddressAbroad.Value; // no condition because all address is required

                        //await _beneficiaryInformationRepo.SaveAsync(beneficiaryModel, 1);

                        #endregion Create BeneficiaryInformation
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
                        vwModel.SpouseModel.LastName = vwModel.SpouseModel.LastName != null ? vwModel.SpouseModel.LastName : string.Empty;
                        vwModel.SpouseModel.FirstName = vwModel.SpouseModel.FirstName != null ? vwModel.SpouseModel.FirstName : string.Empty;

                        await _spouseRepo.SaveAsync(vwModel.SpouseModel);
                    }

                    if (vwModel.Form2PageModel != null)
                    {
                        await _form2PageRepo.SaveAsync(vwModel.Form2PageModel);
                    }
                }

                // last stage pass parameter code

                #region Notification

                var type = vwModel.ApplicantsPersonalInformationModel.Id == 0 ? "Added" : "Updated";
                var actiontype = type;

                var actionlink = $"Applicants/Details/{applicantCode}";

                await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_APPLICANTSREQUESTS, actionlink, actiontype, applicantCode, userId, companyId);

                #endregion Notification

                return Ok(applicantCode);
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

        #endregion API Operations

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

        private static string GenerateRandomPassword()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+";
            Random rand = new Random();

            // Generate random password length between 10 and 12 characters
            int passwordLength = rand.Next(10, 13); // Returns a value between 10 and 12 (exclusive)

            // Hash the current time to introduce some randomness
            string timeStamp = DateTime.Now.Ticks.ToString();

            // Concatenate the GUID with the current time
            string combinedString = Guid.NewGuid().ToString("N").Substring(0, 16) + timeStamp;

            // Use SHA-256 to hash the combined string
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(combinedString);
                byte[] hash = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                // Ensure the password is within the desired length range
                string hashedString = sb.ToString();
                if (hashedString.Length < passwordLength)
                {
                    // If the hashed string is shorter than the desired length, pad it with additional characters
                    while (hashedString.Length < passwordLength)
                    {
                        hashedString += allowedChars[rand.Next(allowedChars.Length)];
                    }
                }
                else if (hashedString.Length > passwordLength)
                {
                    // If the hashed string is longer than the desired length, truncate it
                    hashedString = hashedString.Substring(0, passwordLength);
                }

                return hashedString;
            }
        }

        private async Task<bool> UsernameExistsAsync(string username)
        {
            return (await _userRepo.GetAllAsync()).Any(x => x.UserName == username);
        }

        #endregion Helper Methods
    }
}