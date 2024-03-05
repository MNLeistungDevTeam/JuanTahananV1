using System.Collections.Generic;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;

namespace Template.Web.Models
{
    public class RoleViewModel
    {
        public RoleModel Role { get; set; }
        public List<RoleAccessModel> RoleAccess { get; set; }
        public UserRoleModel UserRole { get; set; }
    }
}
