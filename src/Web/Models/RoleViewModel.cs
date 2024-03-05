using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Dto.UserDto;
using System.Collections.Generic;
using DMS.Domain.Entities;

namespace DMS.Web.Models
{
    public class RoleViewModel
    {
        public RoleModel Role { get; set; }
        public List<RoleAccessModel> RoleAccess { get; set; }
        public UserRoleModel UserRole { get; set; }
    }
}
