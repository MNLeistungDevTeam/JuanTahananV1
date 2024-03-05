using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Dto.UserDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.UserRepository
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
