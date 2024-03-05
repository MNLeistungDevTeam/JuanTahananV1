using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.DocumentDto
{
    public class ApplicationSubmittedDocumentModel
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public int TotalUploadedDocuments { get; set; }
        public string? Description { get; set; }
    }
}
