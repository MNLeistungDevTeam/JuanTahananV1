using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
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
        Task CreateInitialApprovalStatusAsync(int transactionId, string moduleCode, int userId, int companyId, ApprovalStatusType? status = ApprovalStatusType.PendingReview);
        Task<IEnumerable<ApprovalStatusModel?>> GetByReferenceAsync(int referenceId, string referenceType, int companyId);
        Task<ApprovalStatusModel?> GetAsync(int id);
        Task<ApprovalStatusModel?> GetByReferenceModuleCodeAsync(int referenceId, string moduleCode, int companyId);
        Task<ApprovalStatusModel?> GetByReferenceIdAsync(int? referenceId = null, int? companyId = null, int? approvalStatusId = null);
    }
}
