using DMS.Domain.Dto.ModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;
using DMS.Domain.Dto.ModuleStageDto;

namespace DMS.Application.Interfaces.Setup.ModuleRepository
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(int id);

        Task<List<ModuleModel>> SpGetAllUserModules();

        Task<List<ModuleModel>> Module_GetAllModuleList();

        Task<Module> SaveAsync2(ModuleModel model);

        Task DeleteAsync(int id);

        Task BatchDeleteAsync(int[] ids);

        Task<ModuleModel?> GetByCodeAsync(string code);

        Task<List<ModuleModel>> GetAllAsync();

        Task<List<ModuleModel>> GetWithApproversAsync();
        Task<Module> SaveAsync(ModuleModel module, List<ModuleStageModel?> moduleStages, int userId);
        Task<Module> CreateAsync(Module module);
        Task<Module> UpdateAsync(Module module);
    }
}