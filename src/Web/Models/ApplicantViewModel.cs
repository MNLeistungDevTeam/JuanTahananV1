using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.DocumentDto;
using System.Collections.Generic;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Domain.Entities;

namespace DMS.Web.Models
{
    public class ApplicantViewModel
    {
        public ApplicantsPersonalInformationModel ApplicantsPersonalInformationModel { get; set; } = new();
        public LoanParticularsInformationModel LoanParticularsInformationModel { get; set; } = new();
        public CollateralInformationModel CollateralInformationModel { get; set; } = new();
        public BarrowersInformationModel BarrowersInformationModel { get; set; } = new();
        public SpouseModel SpouseModel { get; set; } = new();
        public List<ApplicationSubmittedDocumentModel> ApplicationSubmittedDocumentModels { get; set; } = new();
        public Form2PageModel  Form2PageModel { get; set; } = new();
    }
}
