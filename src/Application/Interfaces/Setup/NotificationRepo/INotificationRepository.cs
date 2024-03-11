using DMS.Domain.Dto.NotificationDto;
using DMS.Domain.Dto.NotificationReceiverDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.NotificationRepo
{
    public interface INotificationRepository
    {
        Task BatchDeleteAsync(int[] ids);
        Task<int> CountUnreadApprovalNotificationAsync(int userId, int companyId);
        Task<int> CountUnreadNotificationAsync(int userId, int companyId);
        Task<int> CountUnreadTransactionNotificationAsync(int userId, int companyId);
        Task<Notification> CreateAsync(Notification notification, int userId);
        Task DeleteAllNotificationAsync(int userId, int companyId);
        Task DeleteAsync(int id);
        Task DeleteNotificationAsync(int id);
        Task<List<Notification>> GetAllAsync();
        Task<IEnumerable<NotificationModel>> GetAnnouncementAsync();
        Task<NotificationModel?> GetAnnouncementByIdAsync(int id);
        Task<NotificationModel?> GetAsync(int id);
        Task<IEnumerable<NotificationReceiverModel>> GetMyNotificationAsync(NotificationFilterModel model, int companyId);
        Task<UserModel?> GetRoleByUserIdAsync(int userId);
        Task<IEnumerable<NotificationReceiverModel>> GetUnreadNotificationAsync(int userId, int pageNumber, int pageSize, int type, int companyId);
        Task<Notification> SaveAsync(Notification notification, int userId);
        Task<Notification> SaveNotificationAsync(NotificationModel nModel, List<int> roles, NotificationType type, int userId);
        Task<Notification> SaveTransactionNotificationAsync(string moduleCode, string actionlink, int userId, string actiontype, string uniquecode, int companyId);
        Task UpdateAllNotificationToTrashAsync(int userid);
        Task<Notification> UpdateAsync(Notification notification, int userId);
        Task UpdateNotificationStatusAsync(int id, bool isRead);
        Task UpdateNotificationToTrashAsync(int id);
    }
}
