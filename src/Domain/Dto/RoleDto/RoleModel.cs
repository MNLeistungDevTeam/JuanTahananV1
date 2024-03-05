using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.RoleDto
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Display(Name = "Code", Prompt = "Input Code")]
        [Required(ErrorMessage = "this field is required")]
        public string? Name { get; set; }
        [Display(Name = "Description", Prompt = "Input Description")]
        [Required(ErrorMessage = "this field is required")]
        public string? Description { get; set; }
        [DisplayName("Disabled")]
        public bool IsDisabled { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }
        public bool AdminAccess { get; set; }
        public DateTime? DateModified { get; set; }
        public int TotalModulesCount { get; set; }
        public int TotalUserCount { get; set; }
    }
}
