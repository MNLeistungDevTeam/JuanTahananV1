using AutoMapper;
using DMS.Application.Interfaces.Setup.ApprovalLogRepo;
using DMS.Domain.Dto.ApprovalLogDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApprovalLogRepo
{
    public class ApprovalLogRepository :IApprovalLogRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ApprovalLog> _contextHelper;
        private readonly IMapper _mapper;

        public ApprovalLogRepository(
            DMSDBContext context,
            IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ApprovalLog>(context);
            _mapper = mapper;
        }

        #region Operation

        public async Task<ApprovalLog> SaveAsync(ApprovalLogModel model)
        {
            var approverLog = _mapper.Map<ApprovalLog>(model);
            if (approverLog.Id == 0)
            {
                approverLog = await CreateAsync(approverLog);
            }
            else
            {
                approverLog = await UpdateAsync(approverLog);
            }
            return approverLog;
        }

        public async Task<ApprovalLog> CreateAsync(ApprovalLog approverLog)
        {
            var result = await _contextHelper.CreateAsync(approverLog);
            return result;
        }

        public async Task<ApprovalLog> UpdateAsync(ApprovalLog approverLog)
        {
            var result = await _contextHelper.UpdateAsync(approverLog);
            return result;
        }

        public async Task DeleteAsync(ApprovalLog approverLog)
        {
            await _contextHelper.DeleteAsync(approverLog);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.ApprovalLogs.FirstOrDefault(d => d.Id == id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.ApprovalLogs.Where(d => ids.Contains(d.Id));
            if (entities != null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        #endregion Operation
    }

}
