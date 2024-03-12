using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.ValidationsHelper;
using DMS.Domain.Dto.OtherDto;

namespace DMS.Domain.Dto.ModuleDto
{
    public class ModuleModel
    {
        public int Id { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "this field is required!")]
        public int ModuleStatusId { get; set; }

        [DisplayName("Parent Module")]
        public int? ParentModuleId { get; set; }

        [Display(Name = "Code", Prompt = "Enter the code...")]
        [Required(ErrorMessage = "this field is required!")]
        public string? Code { get; set; }

        [DisplayName("Bread Title")]
       
        public string? BreadName { get; set; }

        [DisplayName("Module Name")]
        [Required(ErrorMessage = "this field is required!")]
        public string? Description { get; set; }

        [DisplayName("Icon")]
        [Required(ErrorMessage = "this field is required!")]
        public string? Icon { get; set; }

        [DisplayName("Visibility")]
        public bool IsVisible { get; set; }

        [DisplayName("Order")]
        [Required(ErrorMessage = "this field is required!")]
        public int Ordinal { get; set; }

        [DisplayName("Show BreadCrumbs")]
        public bool IsBreaded { get; set; }

        public string? Controller { get; set; }

        [ValidationHelper("Controller", ErrorMessage = "this field is required!")]
        public string? Action { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }

        public DateTime? DateModified { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeletedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? ParentModuleName { get; set; }
        public string? StatusName { get; set; }
        public string? StatusColor { get; set; }

        public string? ParentModule { get; set; }
        [Display(Name ="Module Type")]
        public int? ModuleTypeId { get; set; }
        public List<DropdownModel>? ApprovalRouteTypes { get; set; }

        [DisplayName("Disabled")]
        public bool IsDisabled { get; set; }

        [DisplayName("In Maintenance")]
        public bool InMaintenance { get; set; }

        [DisplayName("Has Approver")]
        public bool WithApprover { get; set; }
        [DisplayName("Approval Type")]
        public int ApprovalRouteTypeId { get; set; }
        public string? ModuleType { get; set; }
        public string? ApprovalRouteType { get; set; }


        
    }
}