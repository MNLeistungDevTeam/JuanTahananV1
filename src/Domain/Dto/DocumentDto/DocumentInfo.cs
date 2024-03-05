using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Dto.DocumentDto
{
    public class DocumentInfo
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? FormattedSize { get; set; }
        public DateTime DateCreated { get; set; }
        public int ApplicantsPersonalInformationId { get; set; }
        public int DocumentTypeId { get; set; }
    }
}
