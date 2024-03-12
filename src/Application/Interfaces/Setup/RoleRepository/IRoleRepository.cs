using DMS.Domain.Dto.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<RoleModel>> GetAllRolesAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<List<Role>?> GetAllAsync();
        Task<Role> SaveAsync(RoleModel model);
        Task<Role> CreateAsync(RoleModel model);
        Task<Role> UpdateAsync(RoleModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
