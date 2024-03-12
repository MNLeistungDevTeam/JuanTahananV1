using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ModuleTypeDto
{
    public class ModuleTypeModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public int Ordinal { get; set; }
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisible { get; set; }
        public bool InMaintenance { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? DateModified { get; set; }
    }
}