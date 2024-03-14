using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Services
{
    public interface INotificationService
    {
        Task NotifyUsersByRoleAccess(string moduleCode, string actionLink, string actionType, string transactionNo, int userId, int companyId);
    }
}
