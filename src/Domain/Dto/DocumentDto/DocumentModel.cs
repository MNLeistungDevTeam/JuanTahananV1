﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.DocumentDto
{
    public class DocumentModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int ReferenceId { get; set; }

        public int ReferenceTypeId { get; set; }

        public string ReferenceNo { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int DocumentTypeId { get; set; }

        public int Size { get; set; }

        public string FileType { get; set; }

        public bool IsFolder { get; set; }

        public int CompanyId { get; set; }

        public bool IsDisabled { get; set; }

        public int CreatedById { get; set; }

        public DateTime DateCreated { get; set; }

        public int ModifiedById { get; set; }

        public DateTime DateModified { get; set; }

        #region Display Properties

        public string? DocuTypeDesc { get; set; }

        #endregion Display Properties
    }
}