using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.ApplicantsDto
{
    public class ApplicantsPersonalInformationModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }
        public int UserId { get; set; }

        [Display(Name = "(HAN) Housing Account Number", Prompt = "Input Number")]
        public string? HousingAccountNumber { get; set; }

        //[Display(Name = "Pag-lBIG MID Number/RTN", Prompt = "Input Number")]
        //[Range(0, 999999999999)]
        [Display(Name = "Pag-IBIG MID Number/RTN", Prompt = "XXXX-XXXX-XXXX")]
        public string? PagibigNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
        public int? CompanyId { get; set; }
        public int? ApprovalStatus { get; set; }

        public int? ApproverRoleId { get; set; }

        public int? EncodedStage { get; set; }
        public int? EncodedStatus { get; set; }
        public int? EncodedPartialStatus { get; set; }

        #region Display Properties

        public string? ApplicantFullName { get; set; }
        public string? ApplicantFirstName { get; set; }
        public string? ApproverFullName { get; set; }
        public string? ApproverFirstName { get; set; }
        public string? ApproverRole { get; set; }
        public string? ApplicantEmail { get; set; }
        public string? PositionName { get; set; }
        public string? ApplicationStatus { get; set; }

        public decimal? IncomeAmount { get; set; }
        public string? Developer { get; set; }
        public string? ProjectLocation { get; set; }
        public string? Unit { get; set; }
        public decimal? LoanAmount { get; set; }
        public int? LoanYears { get; set; }

        //Documents

        public int DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentTypeName { get; set; }
        public string? DocumentLocation { get; set; }
        public string? DocumentName { get; set; }
        public int DocumentSize { get; set; }
        public string? DocumentFileType { get; set; }

        public bool? isRequiredDocumentsUploaded { get; set; }
        public bool? isCanAppliedNewApplication { get; set; }
        public bool? isApplicationCurrentActive { get; set; }

        public string? Remarks { get; set; }

        public string? Stage { get; set; }
        public int? StageNo { get; set; }

        public string? ApproverName { get; set; }

        public string? Email { get; set; }
        public int? ApprovalStatusNumber { get; set; }
        public string? ApprovalStageLimit { get; set; } = string.Empty;

        public DateTime? DateSubmitted { get; set; }
        public DateTime? LastUpdated { get; set; }

        public int? DocumentSequence { get; set; }
        public int? ApproverId { get; set; }
        public int? DocumentParentId { get; set; }
        public int? HasParentId { get; set; }
        public int? HasSubdocument { get; set; }
        public int? SenderId { get; set; }
        public string? FileExtension { get; set; }

        //BeneficiaryInformation

        public int? PropertyProjectId { get; set; }
        public int? PropertyUnitId { get; set; }
        public int? PropertyDeveloperId { get; set; }
        public int? PropertyLocationId { get; set; }

        public string? PropertyProjectName { get; set; }
        public string? PropertyLocationName { get; set; }
        public string? PropertyUnitDescription { get; set; }
        public string? PropertyDeveloperLogo { get; set; }

        #endregion Display Properties
    }
}