using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApprovalLevelDto
{
    public class ApprovalFilterModel
    {
        public int? ReferenceId { get; set; }
        public int? CompanyId { get; set; }

        [DisplayName("Type")]
        public string? ReferenceType { get; set; }

        [DisplayName("Approval Status")]
        public int? ApprovalStatusId { get; set; }

        [DisplayName("Approver")]
        public int? ApproverId { get; set; }

        public string? Status { get; set; }
    }
}
