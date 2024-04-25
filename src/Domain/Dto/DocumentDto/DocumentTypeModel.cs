using System.ComponentModel.DataAnnotations;

namespace DMS.Domain.Dto.DocumentDto
{
    public class DocumentTypeModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        [Display(Name = "File Name", Prompt = "Input Filename")]
        [StringLength(255)]
        [Required(ErrorMessage = "this field is required!")]
        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        [Display(Name = "File Type", Prompt = "Select File Type")]
        public int? FileType { get; set; }

        [Display(Name = "File Parent", Prompt = "Select File Parent")]
        public int? ParentId { get; set; }

        #region Display Properties

        public int? TotalDocumentCount { get; set; }
        public string? VerificationTypeDescription { get; set; }
        public int? VerificationType { get; set; }
        public int? DocumentVerificationId { get; set; }
        public string? FileFormat { get; set; }
        public string? ParentDocumentName { get; set; }

        #endregion Display Properties
    }
}