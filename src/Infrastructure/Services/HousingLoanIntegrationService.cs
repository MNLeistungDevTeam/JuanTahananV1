using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.BasicBeneficiaryDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Hangfire;

namespace DMS.Infrastructure.Services
{
    public class HousingLoanIntegrationService : IHousingLoanIntegrationService
    {
        private readonly IBarrowersInformationRepository _barrowersInformationRepo;
        private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
        private readonly IAuthenticationService _authService;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HousingLoanIntegrationService(IBarrowersInformationRepository barrowersInformationRepo,
            IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
            IAuthenticationService authService,
            IUserRoleRepository userRoleRepo,
            IEmailService emailService,
            INotificationService notificationService,
            IBackgroundJobClient backgroundJobClient)
        {
            _barrowersInformationRepo = barrowersInformationRepo;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
            _authService = authService;
            _userRoleRepo = userRoleRepo;
            _emailService = emailService;
            _notificationService = notificationService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model)
        {
            BarrowersInformationModel barrowerModel = new();
            ApplicantsPersonalInformationModel applicantInfoModel = new();

            #region Create Beneficiary User

            //Create Beneficiary User
            UserModel userModel = new()
            {
                Email = model.Email,
                Password = _authService.GenerateTemporaryPasswordAsync(model.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                UserName = await _authService.GenerateTemporaryUsernameAsync(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                Position = "Beneficiary",
                PagibigNumber = model.PagibigMidNumber
            };

            // validate and  register user
            var userData = await _authService.RegisterUser(userModel);

            userModel.Id = userData.Id;

            //save as benificiary
            await _userRoleRepo.SaveBenificiaryAsync(userData.Id);


            userModel.Action = "created";
            //// make the usage of hangfire
            _backgroundJobClient.Enqueue(() =>   _emailService.SendUserCredential(userModel));

            #endregion Create Beneficiary User

            #region Create Applicant

            applicantInfoModel.UserId = userModel.Id;
            applicantInfoModel.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{userModel.Id}";
            applicantInfoModel.PagibigNumber = model.PagibigMidNumber;
            applicantInfoModel.CompanyId = model.CompanyId;

            var applicantInfoData = await _applicantsPersonalInformationRepo.SaveAsync(applicantInfoModel, userModel.Id);

            #endregion Create Applicant

            #region Barrow Data Transfer

            barrowerModel.LastName = model.LastName;
            barrowerModel.FirstName = model.FirstName;
            barrowerModel.MiddleName = model.MiddleName;
            barrowerModel.MobileNumber = model.MobileNumber;
            barrowerModel.BirthDate = model.BirthDate;
            barrowerModel.MobileNumber = model.MobileNumber;
            barrowerModel.Sex = model.Gender;
            barrowerModel.ApplicantsPersonalInformationId = applicantInfoData.Id;
            barrowerModel.Email = model.Email;
            barrowerModel.PresentUnitName = model.PresentUnitName;
            barrowerModel.PresentBuildingName = model.PresentBuildingName;
            barrowerModel.PresentLotName = model.PresentLotName;
            barrowerModel.PresentSubdivisionName = model.PresentSubdivisionName;
            barrowerModel.PresentBaranggayName = model.PresentBarangayName;
            barrowerModel.PresentMunicipalityName = model.PresentMunicipalityName;
            barrowerModel.PresentProvinceName = model.PresentProvinceName;
            barrowerModel.PresentZipCode = model.PresentZipCode;

            barrowerModel.PermanentUnitName = model.PermanentUnitName;
            barrowerModel.PermanentBuildingName = model.PermanentBuildingName;
            barrowerModel.PermanentLotName = model.PermanentLotName;
            barrowerModel.PermanentSubdivisionName = model.PermanentSubdivisionName;
            barrowerModel.PermanentBaranggayName = model.PermanentBarangayName;
            barrowerModel.PermanentMunicipalityName = model.PermanentMunicipalityName;
            barrowerModel.PermanentProvinceName = model.PermanentProvinceName;
            barrowerModel.PermanentZipCode = model.PermanentZipCode;

            barrowerModel.PropertyDeveloperName = model.PropertyDeveloperName;
            barrowerModel.PropertyLocation = model.PropertyLocation;
            barrowerModel.PropertyUnitLevelName = model.PropertyUnitLevelName;

            barrowerModel.IsPermanentAddressAbroad = true; // no condition because all address is required
            barrowerModel.IsPresentAddressAbroad = true; // no condition because all address is required

            #endregion Barrow Data Transfer

            //save Barrower
            await _barrowersInformationRepo.SaveAsync(barrowerModel);

            #region Notification

            var type = "Added";
            var actiontype = type;

            var actionlink = $"Applicants/HLF068/{applicantInfoData.Code}";

            await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_APPLICANTSREQUESTS, actionlink, actiontype, applicantInfoData.Code, 1, 1);

            #endregion Notification
        }
    }
}