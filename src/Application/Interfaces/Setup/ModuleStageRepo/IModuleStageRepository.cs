using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ModuleStageRepo
{
    public interface IModuleStageRepository
    {
        Task<bool> IsApproverOfModule(int userId, string moduleCode);
        Task<List<ModuleStageModel>> GetByModuleId(int moduleId);

        Task BatchDeleteAsync(int[] ids);

        Task<ModuleStage> CreateAsync(ModuleStage moduleStage, int userId);

        Task<List<ModuleStage>> GetAll();

        Task<ModuleStage?> GetById(int id);

        Task<ModuleStage> UpdateAsync(ModuleStage moduleStage, int userId);
        Task<IEnumerable<ModuleStageModel>> GetByModuleCodeAsync(string moduleCode);
    }
}
