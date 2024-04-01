﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.DocumentVerificationDto
{
    public class DocumentVerificationModel
    {
        public int Id { get; set; }

        public int? DocumentTypeId { get; set; }

        [Display(Name ="Verification Type")]
        public int? Type { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }

        public string? DocumentTypeDescription { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        public int? TotalDocumentCount { get; set; }


        public string? DocumentFileType { get; set; }
    }
}