using AutoMapper;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApprovalStatusRepo 
{
    public class ApprovalStatusRepository:IApprovalStatusRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ApprovalStatus> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly IModuleRepository _moduleRepo;

        public ApprovalStatusRepository(
        DMSDBContext context,
        IMapper mapper,
        ISQLDatabaseService db,
        IModuleRepository moduleRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ApprovalStatus>(context);
            _mapper = mapper;
            _db = db;

            _moduleRepo = moduleRepo;
        }

        #region Operation

        public async Task<ApprovalStatus> SaveAsync(ApprovalStatusModel model)
        {
            var _approvalStatus = _mapper.Map<ApprovalStatus>(model);
            if (_approvalStatus.Id == 0)
            {
                _approvalStatus = await CreateAsync(_approvalStatus);
            }
            else
            {
                _approvalStatus = await UpdateAsync(_approvalStatus);
            }

            return _approvalStatus;
        }

        public async Task<ApprovalStatus> CreateAsync(ApprovalStatus approvalStatus)
        {
            var result = await _contextHelper.CreateAsync(approvalStatus);
            return result;
        }

        public async Task<ApprovalStatus> UpdateAsync(ApprovalStatus approvalStatus)
        {
            var result = await _contextHelper.UpdateAsync(approvalStatus);
            return result;
        }

        public async Task DeleteAsync(ApprovalStatus ApprovalStatus)
        {
            await _contextHelper.DeleteAsync(ApprovalStatus);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.ApprovalStatuses.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.ApprovalStatuses.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
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
                var approvalStatus = await SaveAsync(approvalStatusToSave);
                _context.Entry(approvalStatus).State = EntityState.Detached;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Operation
    }
}