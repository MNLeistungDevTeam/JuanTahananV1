using DMS.Domain.Dto.ModuleStageApproverDto;
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

        Task<ModuleStageApprover> CreateAsync(ModuleStageApproverModel moduleStageApprover, int userId);

        Task<List<ModuleStageApprover>> GetAll();

        Task<ModuleStageApprover?> GetById(int id);

        Task<List<ModuleStageApprover>> GetByModuleStageId(int moduleStageId);

        Task<ModuleStageApprover> SaveAsync(ModuleStageApproverModel moduleStageApprover, int userId);

        Task<ModuleStageApprover> UpdateAsync(ModuleStageApproverModel moduleStageApprover, int userId);
    }
}
