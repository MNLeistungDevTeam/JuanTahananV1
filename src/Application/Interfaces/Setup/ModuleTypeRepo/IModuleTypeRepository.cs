using DMS.Domain.Dto.ModuleTypeDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ModuleTypeRepo
{
    public interface IModuleTypeRepository
    {
        Task<List<ModuleType>> GetAllAsync();

        Task<ModuleType?> GetByIdAsync(int id);

        Task BachDeleteAsync(int[] ids);

        Task<ModuleType> SaveAsync(ModuleTypeModel moduleType, int userId);

        Task<ModuleType> CreateAsync(ModuleType moduleType, int userId);

        Task<ModuleType> UpdateAsync(ModuleType moduleType, int userId);
    }
}