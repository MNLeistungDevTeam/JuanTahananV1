using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ApprovalStatusRepo
{
    public interface IApprovalStatusRepository
    {
        Task BatchDeleteAsync(int[] ids);
        Task DeleteAsync(int id);
        Task<ApprovalStatus> CreateAsync(ApprovalStatus approvalStatus);
        Task<ApprovalStatus> SaveAsync(ApprovalStatusModel model);
        Task<ApprovalStatus> UpdateAsync(ApprovalStatus approvalStatus);
    }
}
