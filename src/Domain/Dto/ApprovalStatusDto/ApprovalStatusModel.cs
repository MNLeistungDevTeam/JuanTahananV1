using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApprovalStatusDto
{
    public class ApprovalStatusModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Prepared By
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Record Id
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// Module Id
        /// </summary>
        public int ReferenceType { get; set; }

        /// <summary>
        /// 0 = For Approval, 1 = Approved, 2 = Canceled
        /// </summary>
        public int Status { get; set; }

        public DateTime LastUpdate { get; set; }
        public string? StatusDescription { get; set; }


        public int ApprovalStatusId { get; set; }   
        public int? ApprovalLevelStatus { get; set; }   
        public int? MaxModuleStageLevel { get; set; }   
        public int? CurrentModuleStageLevel { get; set; }   
        public string? ModuleCode { get; set; }   

    }
}
