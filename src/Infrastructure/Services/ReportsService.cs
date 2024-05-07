using DevExpress.XtraReports.UI;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.BuyerConfirmationRepo;
using DMS.Application.Interfaces.Setup.DocumentRepository;
using DMS.Application.PredefinedReports.BuyerConfirmation;
using DMS.Application.PredefinedReports.HousingLoanApplication;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.ReportDto;
using DMS.Infrastructure.PredefinedReports;
using System.Reflection;

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

        private readonly IDocumentRepository _documentRepo;
        private readonly IBuyerConfirmationRepository _buyerConfirmationRepo;

        public ReportsService(IApplicantsPersonalInformationRepository applicantpersonalInfoRepo,
            IBarrowersInformationRepository barrowersInfoRepo,
            ISpouseRepository spouseRepo,
            ILoanParticularsInformationRepository loanParticularsInfoRepo,
            IForm2PageRepository form2PageRepo,
            ICollateralInformationRepository collateralInfoRepo,
            IDocumentRepository documentRepo,
            IBuyerConfirmationRepository buyerConfirmationRepo)
        {
            _applicantpersonalInfoRepo = applicantpersonalInfoRepo;
            _barrowersInfoRepo = barrowersInfoRepo;
            _spouseRepo = spouseRepo;
            _collateralInfoRepo = collateralInfoRepo;
            _form2PageRepo = form2PageRepo;
            _collateralInfoRepo = collateralInfoRepo;
            _loanParticularsInfoRepo = loanParticularsInfoRepo;
            _documentRepo = documentRepo;
            _buyerConfirmationRepo = buyerConfirmationRepo;
        }

        public async Task<LoanApplicationForm> GenerateHousingLoanForm(string? applicationCode, string? rootFolder)
        {
            try
            {
                var housingFormDetail = new LoanApplicationForm();
                XRSubreport formPage1 = housingFormDetail.Bands[BandKind.Detail].FindControl("subReportFormPage1", true) as XRSubreport;
                XRSubreport formPage2 = housingFormDetail.Bands[BandKind.Detail].FindControl("subReportFormPage2", true) as XRSubreport;

                ApplicantsPersonalInformationModel applicantInfoModel = new();
                BarrowersInformationModel barrowerInfoModel = new();
                SpouseModel spouseInfoModel = new();
                LoanParticularsInformationModel loanParticularsInfoModel = new();
                Form2PageModel form2InfoModel = new();
                CollateralInformationModel collateralInfoModel = new();

                //byte[] formalPicture = new byte[0];

                if (applicationCode != null)
                {
                    //var latestApplicationForm = (await _applicantsPersonalInformationRepo
                    //    .GetAllAsync())

                    //    .Where(x => x.UserId == userId)
                    //    .OrderByDescending(x => x.DateCreated)
                    //    .FirstOrDefault() ?? new ApplicantsPersonalInformation()
                    //    {
                    //        UserId = userId
                    //    };

                    var applicantData = await _applicantpersonalInfoRepo.GetByCodeAsync(applicationCode);

                    if (applicantData != null)
                    {
                        applicantInfoModel = applicantData;
                    }

                    //var applicantDocument = await _documentRepo.GetApplicantDocumentsByCode(applicantData.Code);

                    //if (applicantDocument.Count() > 0)

                    //{
                    //    var documentLocation = applicantDocument.FirstOrDefault(d => d.DocumentTypeId == 7)?.Location ?? "";
                    //    var documentName = applicantDocument.FirstOrDefault(d => d.DocumentTypeId == 7)?.Name ?? "";

                    //    if (!string.IsNullOrWhiteSpace(documentLocation))
                    //    {
                    //        string fileLocation = string.Format("{0}{1}{2}", rootFolder, documentLocation.Replace("/", "\\"), documentName);

                    //        formalPicture = FileMethodHelper.FileToByteArray(fileLocation);
                    //    }
                    //}

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

                List<string> excludedBarrower = new List<string> { "Sex", "MaritalStatus", "HomeOwnerShip", "OccupationStatus", "PreparedMailingAddress" };
                List<string> excludedSpouse = new List<string> { "OccupationStatus" };

                applicantInfoModel = ConvertStringPropertiesToUppercase(applicantInfoModel);
                barrowerInfoModel = ConvertStringPropertiesToUppercase(barrowerInfoModel, excludedBarrower);
                spouseInfoModel = ConvertStringPropertiesToUppercase(spouseInfoModel, excludedSpouse);
                loanParticularsInfoModel = ConvertStringPropertiesToUppercase(loanParticularsInfoModel);
                form2InfoModel = ConvertStringPropertiesToUppercase(form2InfoModel);
                collateralInfoModel = ConvertStringPropertiesToUppercase(collateralInfoModel);

                List<ApplicantInformationReportModel> dataSource = new()

                {
                    new ApplicantInformationReportModel()
                    {
                    //FormalPicture =  formalPicture,

                      ApplicantsPersonalInformationModel =   applicantInfoModel,
                        SpouseModel = spouseInfoModel,
                        BarrowersInformationModel =  barrowerInfoModel,
                        LoanParticularsInformationModel = loanParticularsInfoModel,
                        Form2PageModel = form2InfoModel,
                        CollateralInformationModel = collateralInfoModel,
                    }
                };

                formPage1.ReportSource.DataSource = dataSource;
                formPage2.ReportSource.DataSource = dataSource;
                housingFormDetail.DataSource = dataSource;

                return housingFormDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BuyerConfirmation> GenerateBuyerConfirmationForm(string? buyerConfirmationCode, string? rootFolder)
        {
            try
            {
                var buyerConfirmationFormDetail = new BuyerConfirmation();

                ApplicantsPersonalInformationModel applicantInfoModel = new();
                BarrowersInformationModel barrowerInfoModel = new();
                SpouseModel spouseInfoModel = new();
                LoanParticularsInformationModel loanParticularsInfoModel = new();
                Form2PageModel form2InfoModel = new();
                CollateralInformationModel collateralInfoModel = new();
                BuyerConfirmationModel bcfInforModel = new();

                //byte[] formalPicture = new byte[0];

                if (buyerConfirmationCode != null)
                {

                    var bcfData = await _buyerConfirmationRepo.GetByCodeAsync(buyerConfirmationCode);


                 
                    if (bcfData != null)
                    {

                        bcfInforModel = bcfData;

                    }
                }

                //List<string> excludedBarrower = new List<string> { "Sex", "MaritalStatus", "HomeOwnerShip", "OccupationStatus", "PreparedMailingAddress" };
                //List<string> excludedSpouse = new List<string> { "OccupationStatus" };

                //applicantInfoModel = ConvertStringPropertiesToUppercase(applicantInfoModel);
                //barrowerInfoModel = ConvertStringPropertiesToUppercase(barrowerInfoModel, excludedBarrower);
                //spouseInfoModel = ConvertStringPropertiesToUppercase(spouseInfoModel, excludedSpouse);
                //loanParticularsInfoModel = ConvertStringPropertiesToUppercase(loanParticularsInfoModel);
                //form2InfoModel = ConvertStringPropertiesToUppercase(form2InfoModel);
                //collateralInfoModel = ConvertStringPropertiesToUppercase(collateralInfoModel);

                List<ApplicantInformationReportModel> dataSource = new()

                {
                    new ApplicantInformationReportModel()
                    {
                    //FormalPicture =  formalPicture,

                      ApplicantsPersonalInformationModel =   applicantInfoModel,
                        SpouseModel = spouseInfoModel,
                        BarrowersInformationModel =  barrowerInfoModel,
                        LoanParticularsInformationModel = loanParticularsInfoModel,
                        Form2PageModel = form2InfoModel,
                        CollateralInformationModel = collateralInfoModel,
                        BuyerConfirmationModel = bcfInforModel
                    }
                };
                buyerConfirmationFormDetail.DataSource = dataSource;

                return buyerConfirmationFormDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T ConvertStringPropertiesToUppercase<T>(T obj, List<string>? excludedProperties = null)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string) && property.CanWrite)
                {
                    if (excludedProperties == null || !excludedProperties.Contains(property.Name))
                    {
                        string? value = (string?)property.GetValue(obj);
                        if (value != null)
                        {
                            property.SetValue(obj, value.ToUpper());
                        }
                    }
                }
            }

            return obj;
        }
    }
}