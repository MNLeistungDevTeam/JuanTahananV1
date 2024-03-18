using AutoMapper;
using DMS.Application.Interfaces.Setup.ApprovalLevelRepo;
using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApprovalLevelRepo
{
    public class ApprovalLevelRepository : IApprovalLevelRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ApprovalLevel> _contextHelper;
        private readonly IMapper _mapper;

        public ApprovalLevelRepository(
            DMSDBContext context,
            IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ApprovalLevel>(context);
            _mapper = mapper;
        }



        public async Task<List<ApprovalLevel>> GetByApprovalStatusIdAsync(int approvalStatusId)
        {
            try
            {
                var result = await _context.ApprovalLevels.Where(x => x.ApprovalStatusId == approvalStatusId)
                    .ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region Operation

        public async Task<ApprovalLevel> SaveAsync(ApprovalLevelModel model)
        {
            var approvalLevel = _mapper.Map<ApprovalLevel>(model);
            if (approvalLevel.Id == 0)
            {
                approvalLevel = await CreateAsync(approvalLevel);
            }
            else
            {
                approvalLevel = await UpdateAsync(approvalLevel);
            }

            return approvalLevel;
        }

        public async Task<ApprovalLevel> CreateAsync(ApprovalLevel approvalLevel)
        {
            var result = await _contextHelper.CreateAsync(approvalLevel);
            return result;
        }

        public async Task<ApprovalLevel> UpdateAsync(ApprovalLevel approvalLevel)
        {
            var result = await _contextHelper.UpdateAsync(approvalLevel);
            return result;
        }

        public async Task DeleteAsync(ApprovalLevel approvalLevel)
        {
            await _contextHelper.DeleteAsync(approvalLevel);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.ApprovalLevels.FirstOrDefault(d => d.Id == id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.ApprovalLevels.Where(d => ids.Contains(d.Id));
            if (entities != null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        #endregion Operation
    }

}
