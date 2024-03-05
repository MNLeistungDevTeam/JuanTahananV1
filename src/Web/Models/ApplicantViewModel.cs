using System.Collections.Generic;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Dto.DocumentDto;
using Template.Domain.Entities;

namespace Template.Web.Models
{
    public class ApplicantViewModel
    {
        public ApplicantsPersonalInformationModel ApplicantsPersonalInformation { get; set; } = new();
        public LoanParticularsInformationModel LoanParticularsInformation { get; set; } = new();
        public CollateralInformationModel CollateralInformation { get; set; } = new();
        public BarrowersInformationModel BarrowersInformationModel { get; set; } = new();
        public SpouseModel SpouseModel { get; set; } = new();
        public List<ApplicationSubmittedDocumentModel> ApplicationSubmittedDocumentModels { get; set; } = new();
        public Form2PageModel  Form2PageModel { get; set; } = new();
    }
}
