using AutoMapper;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ModuleDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModuleRepository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Module> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly IModuleStageRepository _moduleStageRepo;

        public ModuleRepository(
            DMSDBContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ISQLDatabaseService db,
            IModuleStageRepository moduleStageRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Module>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
            _moduleStageRepo = moduleStageRepo;
        }

        public async Task<Module?> GetByIdAsync(int id) =>
            await _context.Modules.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ModuleModel>> Module_GetAllModuleList() =>
        (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetAllModules", new { })).ToList();

        public async Task<List<ModuleModel>> SpGetAllUserModules() =>
        (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetAllUserModules", new { userId = _currentUserService.GetCurrentUserId() })).ToList();

        public async Task<ModuleModel?> GetByCodeAsync(string code) =>
            await _db.LoadSingleAsync<ModuleModel, dynamic>("spModule_GetByCode", new { code });

        public async Task<List<ModuleModel>> GetAllAsync() =>
            (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetAll", new { })).ToList();

        public async Task<List<ModuleModel>> GetWithApproversAsync() =>
            (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetWithApprovers", new { })).ToList();

        public async Task<Module> SaveAsync2(ModuleModel model)
        {
            var _module = _mapper.Map<Module>(model);

            if (model.Id == 0)
                _module = await CreateAsync(_module);
            else
                _module = await UpdateAsync(_module);
            return _module;
        }

        public async Task<Module> SaveAsync(ModuleModel module, List<ModuleStageModel> moduleStages, int userId)
        {
            var _module = _mapper.Map<Module>(module);

            if (module.Id == 0)
            {
                _module = await CreateAsync(_module);
            }
            else
            {
                _module = await UpdateAsync(_module);
            }

            var count = 1;
            var stagesToCompare = new List<ModuleStage>();
            if (moduleStages is not null && moduleStages.Any())
            {
                foreach (var moduleStage in moduleStages)
                {
                    moduleStage.ModuleId = _module.Id;
                    moduleStage.Level = count;
                    var _moduleStage = _mapper.Map<ModuleStage>(moduleStage);

                    if (moduleStage.Id == 0)
                    {
                        await _moduleStageRepo.CreateAsync(_moduleStage, userId);
                    }
                    else
                    {
                        await _moduleStageRepo.UpdateAsync(_moduleStage, userId);
                    }

                    stagesToCompare.Add(_moduleStage);
                    count++;
                }

                // clean up for unused stages
                var moduleStagesIds = stagesToCompare.Where(m => m.Id != 0).Select(m => m.Id).ToList();

                if (moduleStagesIds.Any())
                {
                    var toDelete = await _context.ModuleStages
                        .Where(m => m.ModuleId == module.Id && !moduleStagesIds.Contains(m.Id))
                        .Select(m => m.Id)
                        .ToArrayAsync();

                    await _moduleStageRepo.BatchDeleteAsync(toDelete);
                }
            }

            return _module;
        }

        public async Task<Module> CreateAsync(Module module)
        {
            module.DateCreated = DateTime.Now;
            module.CreatedById = _currentUserService.GetCurrentUserId();

            module = await _contextHelper.CreateAsync(module, "DateModified", "ModifiedById");
            return module;
        }

        public async Task<Module> UpdateAsync(Module module)
        {
            module.DateModified = DateTime.Now;
            module.ModifiedById = _currentUserService.GetCurrentUserId();

            module = await _contextHelper.UpdateAsync(module, "DateCreated", "CreatedById");
            return module;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
            {
                entity.DateDeleted = DateTime.Now;
                entity.DeletedById = _currentUserService.GetCurrentUserId();
                if (entity is not null)
                    await _contextHelper.UpdateAsync(entity);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.Modules.Where(m => ids.Contains(m.Id)).ToListAsync();

            if(entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }
    }
}