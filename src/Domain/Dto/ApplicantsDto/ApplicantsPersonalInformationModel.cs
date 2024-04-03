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

        #region Display Properties

        public string? ApplicantFullName { get; set; }
        public string? PositionName { get; set; }
        public string? ApplicationStatus { get; set; }

        public decimal? IncomeAmount { get; set; }
        public string? Developer { get; set; }
        public string? ProjectLocation { get; set; }
        public string? Unit { get; set; }
        public decimal? LoanAmount { get; set; }

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
        #endregion Display Properties
    }
}