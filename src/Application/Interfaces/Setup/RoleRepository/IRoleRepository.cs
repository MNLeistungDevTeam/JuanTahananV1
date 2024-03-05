﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.RoleRepository
{
    public interface IRoleRepository
    {
        Task<List<RoleModel>> SpGetAllRoles();
        Task<Role?> GetByIdAsync(int id);
        Task<List<Role>?> GetAllAsync();
        Task<Role> SaveAsync(RoleModel model);
        Task<Role> CreateAsync(RoleModel model);
        Task<Role> UpdateAsync(RoleModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
