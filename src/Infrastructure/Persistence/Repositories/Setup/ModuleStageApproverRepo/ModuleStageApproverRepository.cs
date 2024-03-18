using AutoMapper;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageApproverRepo;
using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ModuleStageApproverDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModuleStageApproverRepo
{
    public class ModuleStageApproverRepository : IModuleStageApproverRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ModuleStageApprover> _contextHelper;
        private readonly ISQLDatabaseService _db;
        private readonly IMapper _mapper;
        private readonly IModuleRepository _moduleRepo;
        private readonly IModuleStageRepository _moduleStageRepo;

        public ModuleStageApproverRepository(
            DMSDBContext context,
            IMapper mapper,
            ISQLDatabaseService db,
            IModuleRepository moduleRepo,
            IModuleStageRepository moduleStageRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ModuleStageApprover>(context);
            _mapper = mapper;
            _db = db;
            _moduleRepo = moduleRepo;
            _moduleStageRepo = moduleStageRepo;
        }

        public async Task<ModuleStageApprover?> GetById(int id) =>
            await _contextHelper.GetByIdAsync(id);

        public async Task<List<ModuleStageApprover>> GetAll() =>
            await _contextHelper.GetAllAsync();

        public async Task<List<ModuleStageApprover>> GetByModuleStageId(int moduleStageId) =>
            await _context.ModuleStageApprovers.Where(m => m.ModuleStageId == moduleStageId).ToListAsync();

        public async Task<ModuleStageApprover> SaveAsync(ModuleStageApproverModel moduleStageApprover, int userId)
        {
            var _moduleStageApprover = new ModuleStageApprover();

            if (moduleStageApprover.Id == 0)
            {
                _moduleStageApprover = await CreateAsync(moduleStageApprover, userId);
            }
            else
            {
                _moduleStageApprover = await UpdateAsync(moduleStageApprover, userId);
            }

            return _moduleStageApprover;
        }

        public async Task<ModuleStageApprover> CreateAsync(ModuleStageApproverModel moduleStageApprover, int userId)
        {
            moduleStageApprover.CreatedById = userId;
            moduleStageApprover.DateCreated = DateTime.UtcNow;

            var _moduleStageApprover = _mapper.Map<ModuleStageApprover>(moduleStageApprover);
            _moduleStageApprover = await _contextHelper.CreateAsync(_moduleStageApprover);

            return _moduleStageApprover;
        }

        public async Task<ModuleStageApprover> UpdateAsync(ModuleStageApproverModel moduleStageApprover, int userId)
        {
            moduleStageApprover.ModifiedById = userId;
            moduleStageApprover.DateModified = DateTime.UtcNow;

            var _moduleStageApprover = _mapper.Map<ModuleStageApprover>(moduleStageApprover);
            _moduleStageApprover = await _contextHelper.CreateAsync(_moduleStageApprover);

            return _moduleStageApprover;
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.ModuleStageApprovers.Where(m => ids.Contains(m.Id)).ToListAsync();
            if (entities is null)
            {
                return;
            }

            await _contextHelper.BatchDeleteAsync(entities);
        }

        public async Task SaveModuleStageApprover(int moduleId, List<ModuleStageModel> model, int userId)
        {
            var module = await _moduleRepo.GetByIdAsync(moduleId);
            int count = 0; // Initialize counter variable

            if (model is not null && model.Any())
            {
                foreach (var item in model)
                {
                    count++;

                    //Role
                    if (item.ApproverType == 1)
                    {
                        var modulestageModel = new ModuleStageModel()
                        {
                            ModuleId = moduleId,
                            Name = module.Description,
                            Title = item.Title,
                            Level = count,
                            ApproveDesc = item.ApproveDesc,
                            RejectDesc = item.RejectDesc,
                            ReturnStage = item.ReturnStage,
                            RequiredCount = item.RequiredCount,
                        };

                        var modulestage = await _moduleStageRepo.SaveAsync(modulestageModel, userId);

                        ModuleStageApproverModel stageApprover = new()
                        {
                            ModuleStageId = modulestage.Id,
                            RoleId = item.ApproverId
                        };

                        await SaveAsync(stageApprover, userId);
                    }
                    //User
                    else
                    {
                        var modulestageModel = new ModuleStageModel()
                        {
                            Name = module.Description,
                            Title = item.Title,
                            Level = count,
                            ApproveDesc = item.ApproveDesc,
                            RejectDesc = item.RejectDesc,
                            ReturnStage = item.ReturnStage,
                            RequiredCount = item.RequiredCount,
                        };

                        var modulestage = await _moduleStageRepo.SaveAsync(modulestageModel, userId);

                        ModuleStageApproverModel stageApprover = new()
                        {
                            ModuleStageId = modulestage.Id,
                            ApproverId = item.ApproverId
                        };

                        await SaveAsync(stageApprover, userId);
                    }
                }
            }
        }
    }
}