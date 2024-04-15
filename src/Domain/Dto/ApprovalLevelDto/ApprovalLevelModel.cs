using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApprovalLevelDto
{
    public class ApprovalLevelModel
    {
        public int Id { get; set; }

        public int ApprovalStatusId { get; set; }

        public int Status { get; set; }

        public int ApproverId { get; set; }

        /// <summary>
        /// ApprovalLevel
        /// </summary>
        public int Level { get; set; }

        [Required]
        public string? Remarks { get; set; }

        public DateTime DateUpdated { get; set; }

        public int? ModuleStageId { get; set; }

        [Display(Name = "Transaction No.", Prompt = "Transaction No.")]
        public string? TransactionNo { get; set; }

        public int? ApprovalLevelStatus { get; set; }

        public int? TransactionId { get; set; }

        public string? ModuleCode { get; set; }
        
    }
}
