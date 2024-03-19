using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ApprovalLevelRepo
{
    public interface IApprovalLevelRepository
    {
        Task BatchDeleteAsync(int[] ids);
        Task DeleteAsync(int id);
        Task<List<ApprovalLevel>> GetByApprovalStatusIdAsync(int approvalStatusId);
        Task<ApprovalLevel> SaveAsync(ApprovalLevelModel model);
    }
}
