using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApprovalStatusDto
{
    public class ApprovalInfoModel
    {
        public int? TotalPendingReview { get; set; }
        public int? TotalSubmitted { get; set; }
        public int? TotalApprove { get; set; }
        public int? TotalDisApprove { get; set; }
        public int? TotalCount { get; set; }
    }
}