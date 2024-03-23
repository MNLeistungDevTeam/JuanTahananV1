using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.DocumentDto
{
    public class DocumentTypeModel
    {
        public int Id { get; set; }

        [Display(Name = "File Name", Prompt = "Input filename")]
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
        public int? TotalDocumentCount { get; set; }

        public string? VerificationTypeDescription { get; set; }
        public int? VerificationType { get; set; }
        public int? DocumentVerificationId { get; set; }
    }
}