using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.DocumentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ReportDto
{

    public class ApplicantInformationReportModel
    {

        public byte[]? FormalPicture { get; set;  }
        public ApplicantsPersonalInformationModel ApplicantsPersonalInformationModel { get; set; } = new();
        public LoanParticularsInformationModel LoanParticularsInformationModel { get; set; } = new();
        public CollateralInformationModel CollateralInformationModel { get; set; } = new();
        public BarrowersInformationModel BarrowersInformationModel { get; set; } = new();
        public SpouseModel SpouseModel { get; set; } = new();
        public List<ApplicationSubmittedDocumentModel> ApplicationSubmittedDocumentModels { get; set; } = new();
        public Form2PageModel Form2PageModel { get; set; } = new();
        public BuyerConfirmationModel BuyerConfirmationModel { get; set; } = new();
    }
}