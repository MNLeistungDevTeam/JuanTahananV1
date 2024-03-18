﻿using System;
using System.Collections.Generic;
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

        public string? Remarks { get; set; }

        public DateTime DateUpdated { get; set; }

        public int? ModuleStageId { get; set; }
    }
}