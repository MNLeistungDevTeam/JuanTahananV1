using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.NotificationRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DMS.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepo;
        private readonly IRoleAccessRepository _roleAccessRepo;
        private readonly IModuleRepository _moduleRepo;

        private readonly ICompanyRepository _companyRepo;

        public NotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepo,
            IRoleAccessRepository roleAccessRepo, IModuleRepository moduleRepo, ICompanyRepository companyRepo)
        {
            _hubContext = hubContext;
            _notificationRepo = notificationRepo;
            _roleAccessRepo = roleAccessRepo;
            _moduleRepo = moduleRepo;
            _companyRepo = companyRepo;
        }

        //roleaccess or projectId
        public async Task NotifyUsersByRoleAccess(string moduleCode, string actionLink, string actionType, string transactionNo, int userId, int companyId)
        {
            //var module = await _moduleRepo.GetByControllerAsync(moduleController);
            var company = await _companyRepo.GetByIdAsync(companyId);

            string companyCode = "JTH-PH";

            if (company != null)
            {
                companyCode = company.Code;
            }

            //save to database
            await _notificationRepo.SaveTransactionNotificationAsync(moduleCode, actionLink, userId, actionType, transactionNo, companyId);

            var receivers = await _roleAccessRepo.GetRoleByModuleCodeAsync(0, moduleCode);

            var roleNames = receivers.Where(m => m.RoleName != null).Select(m => m.RoleName).Distinct();

            var roleIds = receivers.Where(m => m.RoleId != null).Select(m => m.RoleId).Distinct();

            ///for Projects
            string roles = string.Join(",", roleIds);

            var uniqueReceivers = roleNames;

            var notifData = $"{actionType} Applicant: {transactionNo}";

            // Send notifications to users
            foreach (var receiver in uniqueReceivers)
            {
                await _hubContext.Clients.Group(receiver).SendAsync("AddNotifGroup", companyCode, notifData);
            }
        }
    }
}