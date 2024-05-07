using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.BasicBeneficiaryDto;
using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Enums;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IBeneficiaryInformationRepository _beneficiaryInformationRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IPropertyProjectRepository _propertyProjectRepo;

        public HousingLoanIntegrationService(IBarrowersInformationRepository barrowersInformationRepo,
            IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
            IAuthenticationService authService,
            IUserRoleRepository userRoleRepo,
            IEmailService emailService,
            INotificationService notificationService,
            IBackgroundJobClient backgroundJobClient,
            IBeneficiaryInformationRepository beneficiaryInformationRepo,
            ICompanyRepository companyRepo,
            IPropertyProjectRepository propertyProjectRepo)
        {
            _barrowersInformationRepo = barrowersInformationRepo;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
            _authService = authService;
            _userRoleRepo = userRoleRepo;
            _emailService = emailService;
            _notificationService = notificationService;
            _backgroundJobClient = backgroundJobClient;
            _beneficiaryInformationRepo = beneficiaryInformationRepo;
            _companyRepo = companyRepo;
            _propertyProjectRepo = propertyProjectRepo;
        }

        public async Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model, string? rootFolder)
        {
            BarrowersInformationModel barrowerModel = new();
            BeneficiaryInformationModel beneficiaryModel = new();
            ApplicantsPersonalInformationModel applicantInfoModel = new();

            model.PagibigMidNumber = model.PagibigMidNumber.Replace("-", "");

            #region Create Beneficiary User

            //Create Beneficiary User
            UserModel userModel = new()
            {
                Email = model.Email,
                Password = _authService.GenerateRandomPassword(),  // _authService.GenerateTemporaryPasswordAsync(model.FirstName) sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                UserName = await _authService.GenerateTemporaryUsernameAsync(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Gender = model.Gender,
                Position = "Beneficiary",
                PagibigNumber = model.PagibigMidNumber,
                CompanyId = model.CompanyId
            };

            // validate and  register user
            var userData = await _authService.RegisterUser(userModel);

            userModel.Id = userData.Id;
            userModel.SenderId = 1;

            //save as benificiary
            await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

            userModel.Action = "created";

            //// make the usage of hangfire
            _backgroundJobClient.Enqueue(() => _emailService.SendUserCredential2(userModel, rootFolder));

            #endregion Create Beneficiary User

            #region Create BeneficiaryInformation

            beneficiaryModel.UserId = userData.Id;
            beneficiaryModel.CompanyId = 1;
            beneficiaryModel.PagibigNumber = model.PagibigMidNumber;
            beneficiaryModel.LastName = model.LastName;
            beneficiaryModel.FirstName = model.FirstName;
            beneficiaryModel.MiddleName = model.MiddleName;
            beneficiaryModel.MobileNumber = model.MobileNumber;
            beneficiaryModel.BirthDate = model.BirthDate;
            beneficiaryModel.MobileNumber = model.MobileNumber;
            beneficiaryModel.Sex = model.Gender;
            beneficiaryModel.Email = model.Email;
            beneficiaryModel.PresentUnitName = model.PresentUnitName;
            beneficiaryModel.PresentBuildingName = model.PresentBuildingName;
            beneficiaryModel.PresentLotName = model.PresentLotName;
            beneficiaryModel.PresentStreetName = model.PresentStreetName;
            beneficiaryModel.PresentSubdivisionName = model.PresentSubdivisionName;
            beneficiaryModel.PresentBaranggayName = model.PresentBarangayName;
            beneficiaryModel.PresentMunicipalityName = model.PresentMunicipalityName;
            beneficiaryModel.PresentProvinceName = model.PresentProvinceName;
            beneficiaryModel.PresentZipCode = model.PresentZipCode;

            beneficiaryModel.PermanentUnitName = model.PermanentUnitName;
            beneficiaryModel.PermanentBuildingName = model.PermanentBuildingName;
            beneficiaryModel.PermanentLotName = model.PermanentLotName;
            beneficiaryModel.PermanentStreetName = model.PermanentStreetName;
            beneficiaryModel.PermanentSubdivisionName = model.PermanentSubdivisionName;
            beneficiaryModel.PermanentBaranggayName = model.PermanentBarangayName;
            beneficiaryModel.PermanentMunicipalityName = model.PermanentMunicipalityName;
            beneficiaryModel.PermanentProvinceName = model.PermanentProvinceName;
            beneficiaryModel.PermanentZipCode = model.PermanentZipCode;

            beneficiaryModel.PropertyDeveloperName = model.PropertyDeveloperName;
            beneficiaryModel.PropertyLocation = model.PropertyLocation;
            beneficiaryModel.PropertyUnitLevelName = model.PropertyUnitLevelName;

            beneficiaryModel.IsPermanentAddressAbroad = false; // no condition because all address is required
            beneficiaryModel.IsPresentAddressAbroad = false; // no condition because all address is required

            await _beneficiaryInformationRepo.SaveAsync(beneficiaryModel, 1);

            #endregion Create BeneficiaryInformation

            #region Create Applicant

            applicantInfoModel.UserId = userModel.Id;
            applicantInfoModel.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{userModel.Id}";
            applicantInfoModel.PagibigNumber = model.PagibigMidNumber;
            applicantInfoModel.CompanyId = model.CompanyId;

            //  var applicantInfoData = await _applicantsPersonalInformationRepo.SaveAsync(applicantInfoModel, userModel.Id);

            #endregion Create Applicant

            #region Barrow Data Transfer

            //barrowerModel.LastName = model.LastName;
            //barrowerModel.FirstName = model.FirstName;
            //barrowerModel.MiddleName = model.MiddleName;
            //barrowerModel.MobileNumber = model.MobileNumber;
            //barrowerModel.BirthDate = model.BirthDate;
            //barrowerModel.MobileNumber = model.MobileNumber;
            //barrowerModel.Sex = model.Gender;
            //barrowerModel.ApplicantsPersonalInformationId = applicantInfoData.Id;
            //barrowerModel.Email = model.Email;
            //barrowerModel.PresentUnitName = model.PresentUnitName;
            //barrowerModel.PresentBuildingName = model.PresentBuildingName;
            //barrowerModel.PresentLotName = model.PresentLotName;
            //barrowerModel.PresentSubdivisionName = model.PresentSubdivisionName;
            //barrowerModel.PresentBaranggayName = model.PresentBarangayName;
            //barrowerModel.PresentMunicipalityName = model.PresentMunicipalityName;
            //barrowerModel.PresentProvinceName = model.PresentProvinceName;
            //barrowerModel.PresentZipCode = model.PresentZipCode;

            //barrowerModel.PermanentUnitName = model.PermanentUnitName;
            //barrowerModel.PermanentBuildingName = model.PermanentBuildingName;
            //barrowerModel.PermanentLotName = model.PermanentLotName;
            //barrowerModel.PermanentSubdivisionName = model.PermanentSubdivisionName;
            //barrowerModel.PermanentBaranggayName = model.PermanentBarangayName;
            //barrowerModel.PermanentMunicipalityName = model.PermanentMunicipalityName;
            //barrowerModel.PermanentProvinceName = model.PermanentProvinceName;
            //barrowerModel.PermanentZipCode = model.PermanentZipCode;

            //barrowerModel.PropertyDeveloperName = model.PropertyDeveloperName;
            //barrowerModel.PropertyLocation = model.PropertyLocation;
            //barrowerModel.PropertyUnitLevelName = model.PropertyUnitLevelName;

            //barrowerModel.IsPermanentAddressAbroad = true; // no condition because all address is required
            //barrowerModel.IsPresentAddressAbroad = true; // no condition because all address is required

            #endregion Barrow Data Transfer

            //save Barrower
            // await _barrowersInformationRepo.SaveAsync(barrowerModel);

            #region Notification

            var type = "Added";
            var actiontype = type;

            var actionlink = $"Beneficiary/Details/{model.PagibigMidNumber}";

            await _notificationService.NotifyUsersByRoleAccess(ModuleCodes2.CONST_BENEFICIARY_MGMT, actionlink, actiontype, model.PagibigMidNumber, 1, 1);

            #endregion Notification
        }

        public async Task<IEnumerable<CompanyModel>> GetDevelopers()
        {
            var companies = await _companyRepo.GetCompanies();
            var filteredCompanies = companies
                .Where(company => company.Id != 1 && company.Code != "JTH-PH")
                .ToList();

            return filteredCompanies;
        }





        public async Task<PropertyProjectModel> GetProjectsByCompany(int companyId)
        {
            var projects = await _propertyProjectRepo.GetByCompanyAsync(companyId);
             

            return projects;
        }






        



    }
}