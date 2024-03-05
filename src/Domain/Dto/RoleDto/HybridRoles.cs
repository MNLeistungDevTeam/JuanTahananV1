using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.RoleDto
{
    public class HybridRoles
    {
        public HybridRoles(bool canModify, int index, int id, int roleId, int moduleId, bool canCreate, bool canDelete, bool canRead, string moduleName)
        {
            CanModify = canModify;
            Index = index;
            Id = id;
            RoleId = roleId;
            ModuleId = moduleId;
            CanCreate = canCreate;
            CanDelete = canDelete;
            CanRead = canRead;
            ModuleName = moduleName;
        }

        public bool CanModify { get; set; }
        public bool FullAccess
        {
            get
            {
                if (CanRead && CanDelete && CanModify && CanCreate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int Index { get; set; }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanRead { get; set; }
        public string ModuleName { get; set; }
    }
}
