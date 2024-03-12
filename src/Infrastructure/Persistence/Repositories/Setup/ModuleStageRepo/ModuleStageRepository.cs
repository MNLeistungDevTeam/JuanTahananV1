using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModuleStageRepo
{
    public class ModuleStageRepository : IModuleStageRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ModuleStage> _contextHelper;
        private readonly ISQLDatabaseService _db;

        public ModuleStageRepository(DMSDBContext context, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ModuleStage>(context);
            _db = db;
        }

        public async Task<ModuleStage?> GetById(int id)
        {
            var result = await _contextHelper.GetByIdAsync(id);
            return result;
        }

        public async Task<List<ModuleStage>> GetAll()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        public async Task<bool> IsApproverOfModule(int userId, string moduleCode)
        {
            try
            {
                var data = await _db.LoadSingleAsync<ModuleStageModel, dynamic>("spModuleStage_GetIfApprover", new { userId, moduleCode });

                return data is not null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ModuleStageModel>> GetByModuleId(int moduleId)
        {
            var data = await _db.LoadDataAsync<ModuleStageModel, dynamic>("spModuleStage_GetByModuleId", new { id = moduleId });
            return data.ToList();
        }

        public async Task<IEnumerable<ModuleStageModel>> GetByModuleCodeAsync(string moduleCode) =>
            await _db.LoadDataAsync<ModuleStageModel, dynamic>("spModuleStage_GetByModuleCode", new { moduleCode });



        public async Task<ModuleStage> SaveAsync(ModuleStage moduleStage, int userId)
        {
            if (moduleStage.Id == 0)
            {
                moduleStage = await CreateAsync(moduleStage, userId);
            }
            else
            {
                moduleStage = await UpdateAsync(moduleStage, userId);
            }

            return moduleStage;
        }

        public async Task<ModuleStage> CreateAsync(ModuleStage moduleStage, int userId)
        {
            moduleStage.CreatedById = userId;
            moduleStage.DateCreated = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(moduleStage, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<ModuleStage> UpdateAsync(ModuleStage moduleStage, int userId)
        {
            moduleStage.ModifiedById = userId;
            moduleStage.DateModified = DateTime.UtcNow;
            var result = await _contextHelper.UpdateAsync(moduleStage, "CreatedById", "DateCreated");

            return result;
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.ModuleStages.Where(m => ids.Contains(m.Id));

            await _contextHelper.BatchDeleteAsync(entities);
        }
    }
}
