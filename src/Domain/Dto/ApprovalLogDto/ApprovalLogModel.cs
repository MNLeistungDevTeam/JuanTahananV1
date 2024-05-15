using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ApprovalLogDto
{
    public class ApprovalLogModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Transaction Record Id
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// Module Stage Id
        /// </summary>
        public int StageId { get; set; }

        /// <summary>
        /// 1 = Approved, 2 = Rejected, 3 = Cancelled
        /// </summary>
        public int Action { get; set; }

        public string? Comment { get; set; }

        public int CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ApprovalLevelId { get; set; }
        public int? ApprovalStatusId { get; set; }
    }
}