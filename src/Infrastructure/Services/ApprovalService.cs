using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.ApprovalLevelRepo;
using DMS.Application.Interfaces.Setup.ApprovalLogRepo;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.ModuleStageRepo;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Dto.ApprovalLogDto;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Dto.ModuleStageDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Services
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApplicantsPersonalInformationRepository _applicantsPersonalInformationRepo;
        private readonly IModuleRepository _moduleRepo;
        private readonly IApprovalStatusRepository _approvalStatusRepo;
        private readonly IModuleStageRepository _moduleStageRepo;
        private readonly IApprovalLogRepository _approvalLogRepo;
        private readonly IApprovalLevelRepository _approvalLevelRepo;
        private readonly DMSDBContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;


        public ApprovalService(IModuleRepository moduleRepo,
            IApprovalStatusRepository approvalStatusRepo,
            IModuleStageRepository moduleStageRepo,
            IApprovalLogRepository approvalLogRepo,
            IApprovalLevelRepository approvalLevelRepo,
            DMSDBContext context,
            IUserRepository userRepo,
            IMapper mapper,
            IApplicantsPersonalInformationRepository applicantsPersonalInformationRepo)
        {
            _moduleRepo = moduleRepo;
            _approvalStatusRepo = approvalStatusRepo;
            _approvalLogRepo = approvalLogRepo;
            _approvalLevelRepo = approvalLevelRepo;
            _context = context;
            _moduleStageRepo = moduleStageRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _applicantsPersonalInformationRepo = applicantsPersonalInformationRepo; 
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
                var approvalStatus = await _approvalStatusRepo.GetByReferenceIdAsync(null, null, model.ApprovalStatusId);
                if (approvalStatus is null) { throw new Exception("Approval status not found!"); }

                if (approvalStatus.Status == (int)AppStatusType.Withdrawn)
                    throw new Exception($"Application already Withdrawn!");

                //if (approvalStatus.Status == (int)AppStatusType.Submitted && approvalStatus.Status == (int)AppStatusType.Deferred)
                //    throw new Exception($"Application already {approvalStatus.StatusDescription}!");

                var moduleStages = await _moduleStageRepo.GetByModuleId(approvalStatus.ReferenceType);

                var userInfo = await _userRepo.GetUserAsync(approverId);
                var moduleStage = moduleStages.FirstOrDefault(m => m.RoleId == userInfo.UserRoleId);

                int[] statusExclusions = { (int)AppStatusType.Submitted, (int)AppStatusType.Withdrawn, (int)AppStatusType.PostSubmitted, (int)AppStatusType.Discontinued };

                if (!statusExclusions.Contains(model.Status) && moduleStage == null)
                {
                    throw new Exception($"The Current User is not Approver!");
                }

                model.Level = moduleStage is not null ? moduleStage.Level : 0;
                model.ModuleStageId = moduleStage is not null ? moduleStage.Id : 0;

                //ApprovalLevel approvalLevel = new();
                int approvalLevelId = 0;
                if (model.Status == (int)AppStatusType.DeveloperVerified || model.Status == (int)AppStatusType.PagibigVerified 
                    || model.Status == (int)AppStatusType.Deferred || model.Status == (int)AppStatusType.PagibigConfirmed || model.Status == (int)AppStatusType.DeveloperConfirmed || model.Status == (int)AppStatusType.Disqualified)
                {
                    var approvalLevelData = await _approvalLevelRepo.SaveAsync(model);
                    approvalLevelId = approvalLevelData.Id;
                }

                var log = new ApprovalLogModel()
                {
                    ReferenceId = approvalStatus.ReferenceId,
                    StageId = model.ModuleStageId ?? 0,
                    Action = model.Status,
                    Comment = model.Remarks,
                    ApprovalLevelId = approvalLevelId
                };
                await _approvalLogRepo.SaveAsync(log, approverId);

                await UpdateApprovalStatus(approvalStatus, model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateApprovalStatusForCancel(ApprovalStatusModel model, string? historyComment, int userId)
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

        #region Private Helper Methods

        private async Task UpdateApprovalStatus(ApprovalStatusModel approvalStatus, ApprovalLevelModel aplModel)
        {
            var approvalLevel = _mapper.Map<ApprovalLevel>(aplModel);

            try
            {
                var result = await _context.vwModuleStageApprovalStatuses.Where(x => x.ApprovalStatusId == approvalStatus.Id)
                    .ToListAsync();

                //if (result.Any(x => x.ApprovalLevelId == null)) { return; }
                //if (approvalLevel.Status == (int)ApprovalStatusType.Approved)
                //{
                //    var lastStage = result.OrderByDescending(x => x.Level).FirstOrDefault();
                //    if (lastStage == null) { return; }
                //    if (lastStage.Id != approvalLevel.ModuleStageId)
                //    {
                //        approvalStatus.Status = (int)ApprovalStatusType.PendingReview;
                //        await UpdateTransactionApprovalStatus(approvalStatus, approvalLevel.ApproverId);
                //        var updated = await _approvalStatusRepo.SaveAsync(approvalStatus);

                //        return;
                //    }
                //}

                approvalStatus.Status = approvalLevel.Status;
                await UpdateTransactionApprovalStatus(approvalStatus, approvalLevel.ApproverId);
                var statusSaved = await _approvalStatusRepo.SaveAsync(approvalStatus);
            }
            catch (Exception)
            {
                await _approvalLevelRepo.DeleteAsync(approvalLevel.Id);
                throw;
            }
        }

        private async Task UpdateTransactionApprovalStatus(ApprovalStatusModel approvalStatus, int approverId)
        {
            try
            {
                if (approvalStatus == null) { return; }

                var module = await _moduleRepo.GetByIdAsync(approvalStatus.ReferenceType);
                if (module == null) { return; }

                var user = await _userRepo.GetByIdAsync(approverId);

                int[] statusInclusions = { (int)AppStatusType.Withdrawn, (int)AppStatusType.Deferred, (int)AppStatusType.DeveloperVerified, (int)AppStatusType.PagibigVerified };

                if (module.Code == ModuleCodes2.CONST_APPLICANTSREQUESTS)
                {
                    var budget = await _applicantsPersonalInformationRepo.GetByIdAsync(approvalStatus.ReferenceId);
                    //var budgetInfo = await _applicantsPersonalInformationRepo.GetByTransactionId(budget.Id, budget.CompanyId);
                    if (budget == null) { throw new Exception("Referencing record not found! Unable to proceed."); }
                    //if (user.UserName != budgetInfo.LockedUserName && budgetInfo.IsLocked.Value)
                    //{
                    //    throw new Exception($"{budgetInfo.TransactionNo} is being used by {budgetInfo.LockedUserName}!");
                    //}

                    budget.ApprovalStatus = approvalStatus.Status;
                    budget = await _applicantsPersonalInformationRepo.UpdateAsync(budget, approverId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Private Helper Methods
    }
}