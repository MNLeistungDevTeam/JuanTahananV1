using DMS.Domain.Dto.ModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ModuleRepository
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(int id);
        Task<List<ModuleModel>> SpGetAllUserModules();
        Task<List<ModuleModel>> Module_GetAllModuleList();
        Task<Module> SaveAsync(ModuleModel model);
        Task<Module> CreateAsync(ModuleModel model);
        Task<Module> UpdateAsync(ModuleModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
