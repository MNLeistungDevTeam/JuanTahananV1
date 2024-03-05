using DMS.Domain.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.UserRepository
{
    public interface IUserRoleRepository
    {
        Task<List<UserRoleModel>> SpGetAllRoles();
        Task<UserRole?> GetByIdAsync(int id);
        Task<UserRole?> GetUserRoleAsync(int UserId);
        Task<List<UserRole>?> GetAllAsync();
        Task<UserRole> SaveAsync(UserRoleModel model);
        Task<UserRole> CreateAsync(UserRoleModel model);
        Task<UserRole> UpdateAsync(UserRoleModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
