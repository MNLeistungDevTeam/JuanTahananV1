using DMS.Domain.Dto.ApprovalLevelDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Services
{
    public interface IApprovalService
    {
        Task CreateInitialApprovalStatusAsync(int transactionId, string moduleCode, int userId, int companyId, ApprovalStatusType? status = ApprovalStatusType.PendingReview);

        Task SaveApprovalLevel(ApprovalLevelModel model, int approverId, int companyId);
    }
}