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

        public async Task<IEnumerable<ModuleStageApproverModel>> GetAllAsync() =>

             await _db.LoadDataAsync<ModuleStageApproverModel, dynamic>("spModuleStageApprover_GetAll", new { });

        public async Task<List<ModuleStageApprover>> GetByModuleStageId(int moduleStageId) =>
            await _context.ModuleStageApprovers.Where(m => m.ModuleStageId == moduleStageId).ToListAsync();

        public async Task<ModuleStageApprover> SaveAsync(ModuleStageApproverModel model, int userId)
        {
            var _moduleStageApprover = _mapper.Map<ModuleStageApprover>(model);

            if (_moduleStageApprover.Id == 0)
            {
                _moduleStageApprover = await CreateAsync(_moduleStageApprover, userId);
            }
            else
            {
                _moduleStageApprover = await UpdateAsync(_moduleStageApprover, userId);
            }

            return _moduleStageApprover;
        }

        public async Task<ModuleStageApprover> CreateAsync(ModuleStageApprover moduleStageApprover, int userId)
        {
            moduleStageApprover.CreatedById = userId;
            moduleStageApprover.DateCreated = DateTime.Now;

            moduleStageApprover = await _contextHelper.CreateAsync(moduleStageApprover,"ModifiedById","DateModified");

            return moduleStageApprover;
        }

        public async Task<ModuleStageApprover> UpdateAsync(ModuleStageApprover moduleStageApprover, int userId)
        {
            moduleStageApprover.ModifiedById = userId;
            moduleStageApprover.DateModified = DateTime.Now;

            moduleStageApprover = await _contextHelper.UpdateAsync(moduleStageApprover,"CreatedById","DateCreated");

            return moduleStageApprover;
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
            int count = 1; // Initialize counter variable
            var stagesToCompare = new List<ModuleStage>();
            if (model is not null && model.Any())
            {
                foreach (var item in model)
                {
                    //Role
                    if (item.ApproverType == 1)
                    {
                        var modulestageModel = new ModuleStageModel()
                        {
                            Id = item.Id,
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
                            Id = item.ModuleStageApproverId,
                            ModuleStageId = modulestage.Id,
                            RoleId = item.ApproverId
                        };

                        await SaveAsync(stageApprover, userId);

                        stagesToCompare.Add(modulestage);
                    }
                    //User
                    else
                    {
                        var modulestageModel = new ModuleStageModel()
                        {
                            Id = item.Id,
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
                            Id = item.ModuleStageApproverId,
                            ModuleStageId = modulestage.Id,
                            ApproverId = item.ApproverId
                        };

                        stagesToCompare.Add(modulestage);

                        await SaveAsync(stageApprover, userId);
                    }

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
                    // Assuming GetAll() returns IEnumerable or IQueryable
                    var approverData = await GetAllAsync();

                    // Extract ModuleStageIds into an array of integers
                    var approverToDeleteIds = approverData
                        .Where(approver => toDelete.Contains(approver.ModuleStageId))
                        .Select(approver => approver.ModuleStageId)
                        .ToArray();

                    // Perform batch deletion
                    await BatchDeleteAsync(approverToDeleteIds);
                }
            }
        }
    }
}