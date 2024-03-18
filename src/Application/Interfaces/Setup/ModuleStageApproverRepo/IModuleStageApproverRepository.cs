using DMS.Domain.Dto.ModuleStageApproverDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ModuleStageApproverRepo
{
    public interface IModuleStageApproverRepository
    {
        Task BatchDeleteAsync(int[] ids);

        Task<List<ModuleStageApprover>> GetAll();

        Task<IEnumerable<ModuleStageApproverModel>> GetAllAsync();

        Task<ModuleStageApprover?> GetById(int id);

        Task<List<ModuleStageApprover>> GetByModuleStageId(int moduleStageId);

        Task<ModuleStageApprover> SaveAsync(ModuleStageApproverModel moduleStageApprover, int userId);

        Task SaveModuleStageApprover(int moduleId, List<ModuleStageModel> model, int userId);
    }
}