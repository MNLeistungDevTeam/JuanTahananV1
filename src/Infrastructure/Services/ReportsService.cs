using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.PredefinedReports;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.ReportDto;

namespace DMS.Infrastructure.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IApplicantsPersonalInformationRepository _applicantpersonalInfoRepo;
        private readonly IBarrowersInformationRepository _barrowersInfoRepo;
        private readonly ISpouseRepository _spouseRepo;
        private readonly ILoanParticularsInformationRepository _loanParticularsInfoRepo;
        private readonly IForm2PageRepository _form2PageRepo;
        private readonly ICollateralInformationRepository _collateralInfoRepo;

        public ReportsService(IApplicantsPersonalInformationRepository applicantpersonalInfoRepo,
            IBarrowersInformationRepository barrowersInfoRepo,
            ISpouseRepository spouseRepo,
            ILoanParticularsInformationRepository loanParticularsInfoRepo,
            IForm2PageRepository form2PageRepo,
            ICollateralInformationRepository collateralInfoRepo)
        {
            _applicantpersonalInfoRepo = applicantpersonalInfoRepo;
            _barrowersInfoRepo = barrowersInfoRepo;
            _spouseRepo = spouseRepo;
            _collateralInfoRepo = collateralInfoRepo;
            _form2PageRepo = form2PageRepo;
            _collateralInfoRepo = collateralInfoRepo;
            _loanParticularsInfoRepo = loanParticularsInfoRepo;
        }

        public async Task<HousingLoanApplicationForm> GenerateHousingLoanForm(int userId)
        {
            try
            {
                var housingFormDetail = new HousingLoanApplicationForm();

                ApplicantsPersonalInformationModel applicantInfoModel = new();
                BarrowersInformationModel barrowerInfoModel = new();
                SpouseModel spouseInfoModel = new();
                LoanParticularsInformationModel loanParticularsInfoModel = new();
                Form2PageModel form2InfoModel = new();
                CollateralInformationModel collateralInfoModel = new();

                if (userId != 0)
                {
                    //var latestApplicationForm = (await _applicantsPersonalInformationRepo
                    //    .GetAllAsync())
                    //    .Where(x => x.UserId == userId)
                    //    .OrderByDescending(x => x.DateCreated)
                    //    .FirstOrDefault() ?? new ApplicantsPersonalInformation()
                    //    {
                    //        UserId = userId
                    //    };

                    var applicantData = await _applicantpersonalInfoRepo.GetByUserAsync(userId);

                    if (applicantData != null)
                    {
                        applicantInfoModel = applicantData;
                    }

                    var loanParticularsData = await _loanParticularsInfoRepo.GetByApplicantIdAsync(applicantData.Id);

                    if (loanParticularsData != null)
                    {
                        loanParticularsInfoModel = loanParticularsData;
                    }

                    var collateralData = await _collateralInfoRepo.GetByApplicantIdAsync(applicantData.Id);

                    if (collateralData != null)
                    {
                        collateralInfoModel = collateralData;
                    }

                    var barrowerData = await _barrowersInfoRepo.GetByApplicantIdAsync(applicantData.Id);

                    if (barrowerData != null)
                    {
                        barrowerInfoModel = barrowerData;
                    }

                    var spouseData = await _spouseRepo.GetByApplicantIdAsync(applicantData.Id);

                    if (spouseData != null)
                    {
                        spouseInfoModel = spouseData;
                    }

                    var form2Data = await _form2PageRepo.GetByApplicantIdAsync(applicantData.Id);

                    if (form2Data != null)
                    {
                        form2InfoModel = form2Data;
                    }
                }

                List<ApplicantInformationReportModel> dataSource = new()

                {
                    new ApplicantInformationReportModel()
                    {
                        ApplicantsPersonalInformationModel =  applicantInfoModel,
                        SpouseModel = spouseInfoModel,
                        BarrowersInformationModel = barrowerInfoModel,
                        LoanParticularsInformationModel = loanParticularsInfoModel,
                        Form2PageModel = form2InfoModel,
                        CollateralInformationModel = collateralInfoModel,
                    }
                };

                housingFormDetail.DataSource = dataSource;

                return housingFormDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}