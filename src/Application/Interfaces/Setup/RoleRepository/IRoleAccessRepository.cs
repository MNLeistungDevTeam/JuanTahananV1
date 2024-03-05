using DMS.Domain.Dto.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.RoleRepository
{
    public interface IRoleAccessRepository
    {
        Task<RoleAccess?> GetByIdAsync(int id);
        Task<List<RoleAccess>?> GetAllAsync();
        Task<RoleAccess?> GetRoleAccessAsync(int roleId,int ModuleId);
        Task<RoleAccess> SaveAsync(RoleAccessModel model);
        Task<RoleAccess> CreateAsync(RoleAccessModel model);
        Task<RoleAccess> UpdateAsync(RoleAccessModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
