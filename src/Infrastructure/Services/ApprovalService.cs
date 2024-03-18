using DMS.Application.Interfaces.Setup.ApprovalLevelRepo;
using DMS.Application.Interfaces.Setup.ApprovalLogRepo;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Dto.ApprovalLogDto;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;

namespace DMS.Infrastructure.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IModuleRepository _moduleRepo;
        private readonly IApprovalStatusRepository _approvalStatusRepo;
        private readonly IModuleStageRepository _moduleStageRepo;
        private readonly IApprovalLogRepository _approvalLogRepo;
        private readonly IApprovalLevelRepository _approvalLevelRepo;
        private readonly DMSDBContext _context;

        public ApprovalService(IModuleRepository moduleRepo,
            IApprovalStatusRepository approvalStatusRepo,
            IModuleStageRepository moduleStageRepo,
            IApprovalLogRepository approvalLogRepo,
            IApprovalLevelRepository approvalLevelRepo,
            DMSDBContext context)
        {
            _moduleRepo = moduleRepo;
            _approvalStatusRepo = approvalStatusRepo;
            _approvalLogRepo = approvalLogRepo;
            _approvalLevelRepo = approvalLevelRepo;
            _context = context;
            _moduleStageRepo = moduleStageRepo;
        }

        public async Task CreateInitialApprovalStatusAsync(int transactionId, string moduleCode, int userId, int companyId, ApprovalStatusType? status = ApprovalStatusType.PendingReview)
        {
            try
            {
                var module = await _moduleRepo.GetByCodeAsync(moduleCode);
                if (module == null) { throw new Exception("Module not found!"); }

                ApprovalStatusModel approvalStatusToSave = new ApprovalStatusModel()
                {
                    UserId = userId,
                    ReferenceId = transactionId,
                    ReferenceType = module.Id,
                    Status = (int)(status ?? ApprovalStatusType.PendingReview),
                    LastUpdate = DateTime.Now
                };
                var approvalStatus = await _approvalStatusRepo.SaveAsync(approvalStatusToSave);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveApprovalLevel(ApprovalLevelModel model, int approverId, int companyId)
        {
            try
            {
                //checker if exist in records
                var approvalStatus = await _approvalStatusRepo.GetAsync(model.ApprovalStatusId);
                if (approvalStatus is null) { throw new Exception("Approval status not found!"); }

                if (approvalStatus.Status == (int)ApprovalStatusType.Cancelled)
                    throw new Exception($"Transaction already Cancelled!");

                if (approvalStatus.Status != (int)ApprovalStatusType.PendingReview && approvalStatus.Status != (int)ApprovalStatusType.Returned)
                    throw new Exception($"Transaction already {approvalStatus.StatusDescription}!");

                var moduleStages = await _moduleStageRepo.GetByModuleId(approvalStatus.ReferenceType);

                if (moduleStages == null)
                {
                    return;
                }

                var moduleStage = moduleStages.FirstOrDefault();

                if (model.Status != (int)ApprovalStatusType.Cancelled && moduleStage == null) { throw new Exception("User not allowed to approve transaction!"); }

                model.Level = moduleStage is not null ? moduleStage.Level : 0;
                model.ModuleStageId = moduleStage is not null ? moduleStage.Id : 0;

                ApprovalLevel approvalLevel = new();
                if (model.Status == (int)ApprovalStatusType.Approved || model.Status == (int)ApprovalStatusType.Disapproved)
                {
                    approvalLevel = await _approvalLevelRepo.SaveAsync(model);
                }

                var log = new ApprovalLogModel()
                {
                    ReferenceId = approvalStatus.ReferenceId,
                    StageId = model.ModuleStageId ?? 0,
                    Action = model.Status,
                    Comment = model.Remarks,
                    ApprovalLevelId = approvalLevel.Id
                };
                await _approvalLogRepo.SaveAsync(log, approverId);

                await UpdateApprovalStatus(approvalStatus, null, approverId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateApprovalStatus(ApprovalStatusModel model, string? historyComment, int userId)
        {
            try
            {
                var approvalStatus = await _approvalStatusRepo.SaveAsync(model);
                var moduleStages = await _moduleStageRepo.GetByModuleId(approvalStatus.ReferenceType);
                var approvalLevels = await _approvalLevelRepo.GetByApprovalStatusIdAsync(approvalStatus.Id);

                ModuleStageModel? moduleStage = new();
                if (approvalLevels != null) { return; }

                moduleStage = moduleStages.OrderBy(x => x.Level).FirstOrDefault();

                ApprovalLogModel log = new()
                {
                    ReferenceId = approvalStatus.ReferenceId,
                    StageId = moduleStage.Id,
                    Comment = historyComment,
                    ApprovalLevelId = 0
                };
                await _approvalLogRepo.SaveAsync(log, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}