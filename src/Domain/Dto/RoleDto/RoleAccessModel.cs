using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.RoleDto
{
    public class RoleAccessModel
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }

        public bool CanCreate { get; set; }
        public bool CanModify { get; set; }
        public bool CanCancel { get; set; }
        public bool CanPrint { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }
        public bool FullAccess { get; set; }

        public bool IsApprover { get; set; }

        public int? CreatedById { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? DateModified { get; set; }

        #region Module

        // Module
        public int ModuleId { get; set; }

        public string? ModuleName { get; set; }
        public string? ModuleCode { get; set; }
        public int ParentModuleId { get; set; }
        public bool HasSubModule { get; set; }
        public string? ModuleController { get; set; }
        public string? ModuleAction { get; set; }
        public string? ModuleIcon { get; set; }
        public int ModuleOrdinal { get; set; }
        public bool InMaintenance { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }

        // Module Type
        public int ModuleTypeId { get; set; }

        public string? ModuleType { get; set; }
        public string? ModuleTypeIcon { get; set; }
        public string? ModuleTypeOrder { get; set; }
        public string? ModuleTypeController { get; set; }
        public string? ModuleTypeAction { get; set; }
        public bool ModuleTypeIsVisible { get; set; }
        public bool ModuleTypeIsDisabled { get; set; }
        public bool ModuleTypeInMaintenance { get; set; }
        public string? AccessString { get; set; }

        #endregion Module

        public RoleAccessModel()
        {
            CanCreate = false;
            CanModify = false;
            CanCancel = false;
            CanPrint = false;
            CanDelete = false;
            CanRead = false;
            FullAccess = false;
            IsApprover = false;
        }

        public int UserId { get; set; }
    }
}