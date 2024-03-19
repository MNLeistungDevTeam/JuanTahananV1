using DMS.Domain.Dto.ApprovalLogDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.ApprovalLogRepo
{
    public interface IApprovalLogRepository
    {
        Task<ApprovalLog> SaveAsync(ApprovalLogModel model,int userId);
    }
}
