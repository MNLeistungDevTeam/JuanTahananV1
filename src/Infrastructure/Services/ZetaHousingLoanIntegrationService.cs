using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Dto.ZetaHousingLoanModelDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Services
{
    public class ZetaHousingLoanIntegrationService : IZetaHousingLoanIntegrationService
    {
        private readonly IBarrowersInformationRepository _barrowersInformationRepo;
        private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
        private readonly IAuthenticationService _authService;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IEmailService _emailService;

        public ZetaHousingLoanIntegrationService(IBarrowersInformationRepository barrowersInformationRepo,
            IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo,
            IAuthenticationService authService,
            IUserRoleRepository userRoleRepo,
            IEmailService emailService)
        {
            _barrowersInformationRepo = barrowersInformationRepo;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo;
            _authService = authService;
            _userRoleRepo = userRoleRepo;
            _emailService = emailService;
        }

        public async Task SaveBeneficiaryAsync(ZetaHousingLoanModel model)
        {
            BarrowersInformationModel barrowerModel = new();
            ApplicantsPersonalInformationModel applicantInfoModel = new();
            


            if (model.Id == 0)
            {
                UserModel userModel = new()
                {
                    Email = model.Email,
                    Password = _authService.GenerateTemporaryPasswordAsync(model.FirstName), //sample output JohnDoe9a6d67fc51f747a76d05279cbe1f8ed0
                    UserName = await _authService.GenerateTemporaryUsernameAsync(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    Position = "Beneficiary"
                };

                // validate and  register user
                var userData = await _authService.RegisterUser(userModel);

                userModel.Id = userData.Id;

                //save as benificiary
                await _userRoleRepo.SaveBenificiaryAsync(userData.Id);

                //// make the usage of hangfire
                await _emailService.SendUserInfo(userModel);

                applicantInfoModel.UserId = userModel.Id;
                applicantInfoModel.Code = $"{DateTime.Now.ToString("MMddyyyy")}-{userModel.Id}";
                applicantInfoModel.PagibigNumber = model.PagibigMidNumber;

                var applicantInfoData = await _applicantsPersonalInformationRepo.SaveAsync(applicantInfoModel,0);

                barrowerModel.LastName = model.LastName;
                barrowerModel.FirstName = model.FirstName;
                barrowerModel.MiddleName = model.MiddleName;
                barrowerModel.MobileNumber = model.MobileNumber;
                barrowerModel.BirthDate = model.BirthDate;
                barrowerModel.MobileNumber = model.MobileNumber;
                barrowerModel.Sex = model.Gender;
                barrowerModel.ApplicantsPersonalInformationId = applicantInfoData.Id;

                await _barrowersInformationRepo.SaveAsync(barrowerModel);
            }
        }
    }
}