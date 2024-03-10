using AutoMapper;
using DevExpress.Data.Extensions;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Interfaces.Setup.NotificationReceiverRepo;
using DMS.Application.Interfaces.Setup.NotificationRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.NotificationDto;
using DMS.Domain.Dto.NotificationReceiverDto;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.NotificationRepo
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Notification> _contextHelper;
        private readonly ISQLDatabaseService _db;
        private readonly IMapper _mapper;
        private readonly INotificationReceiverRepository _notificationReceiverRepo;
        private readonly IModuleRepository _moduleRepo;
        private readonly IUserRepository _userRepo;
        private readonly IRoleAccessRepository _roleAccessRepo;

        public NotificationRepository(
            DMSDBContext context,
            ISQLDatabaseService db,
            IMapper mapper,
            INotificationReceiverRepository notificationReceiverRepo,
            IModuleRepository moduleRepo,
            IUserRepository userRepo,
            IRoleAccessRepository roleAccessRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Notification>(context);
            _db = db;
            _mapper = mapper;
            _notificationReceiverRepo = notificationReceiverRepo;
            _moduleRepo = moduleRepo;
            _userRepo = userRepo;
            _roleAccessRepo = roleAccessRepo;
        }

        #region Get Methods

        public async Task<IEnumerable<NotificationReceiverModel>> GetUnreadNotificationAsync(int userId, int pageNumber, int pageSize, int type, int companyId) =>
            await _db.LoadDataAsync<NotificationReceiverModel, dynamic>("spNotification_GetUnread", new { userId, pageNumber, pageSize, type, companyId });

        public async Task<IEnumerable<NotificationReceiverModel>> GetMyNotificationAsync(NotificationFilterModel model, int companyId) =>
            await _db.LoadDataAsync<NotificationReceiverModel, dynamic>("spNotification_GetByUserId", new { model.UserId, model.IsRead, model.IsDelete, companyId });

        public async Task<NotificationModel?> GetAsync(int id) =>
            await _db.LoadSingleAsync<NotificationModel, dynamic>("spNotification_GetById", new { id });

        public async Task<List<Notification>> GetAllAsync()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        public async Task<int> CountUnreadNotificationAsync(int userId, int companyId) =>
        await _db.LoadSingleAsync<int, dynamic>("spNotification_CountUnread", new { userId, companyId });

        public async Task<int> CountUnreadTransactionNotificationAsync(int userId, int companyId) =>
        await _db.LoadSingleAsync<int, dynamic>("spNotification_CountUnreadTransaction", new { userId, companyId });

        public async Task<int> CountUnreadApprovalNotificationAsync(int userId, int companyId) =>
        await _db.LoadSingleAsync<int, dynamic>("spNotification_CountUnreadApprovalTransaction", new { userId, companyId });

        public async Task<UserModel?> GetRoleByUserIdAsync(int userId) =>
            await _db.LoadSingleAsync<UserModel, dynamic>("spUser_GetRoleByUserId", new { userId });

        public async Task<IEnumerable<NotificationModel>> GetAnnouncementAsync() =>
            await _db.LoadDataAsync<NotificationModel, dynamic>("spNotification_GetAnnouncement", new { });

        public async Task<NotificationModel?> GetAnnouncementByIdAsync(int id) =>
           await _db.LoadSingleAsync<NotificationModel, dynamic>("spNotification_GetAnnouncementById", new { id });

        #endregion Get Methods

        public async Task<Notification> SaveNotificationAsync(NotificationModel nModel, List<int?> roles, NotificationType type, int userId)
        {
            var notification = _mapper.Map<Notification>(nModel);

            List<User> users = new();

            //if (type == NotificationType.Role)
            //{                                           //userids from modulecode access with existing userroleIds
            //    users = await _context.Users.Where(m => roles.Contains(m.UserRoleId)).ToListAsync();
            //}
            if (type == NotificationType.User)
            {
                users = await _context.Users.Where(m => roles.Contains(m.Id)).ToListAsync();
            }
            else if (type == NotificationType.Approval)
            {
                users = await _context.Users.Where(m => roles.Contains(m.Id)).ToListAsync();
            }

            notification = await SaveAsync(notification, userId);

            foreach (var item in users)
            {
                NotificationReceiver receiver = new()
                {
                    ReceiverId = item.Id,
                    NotifId = notification.Id,
                    ReceiverType = (int)type
                };

                await _notificationReceiverRepo.SaveAsync(receiver, userId);
            }

            return notification;
        }

        //by module access
        public async Task<Notification> SaveTransactionNotificationAsync(string moduleCode, string actionlink, int userId, string actiontype, string uniquecode, int companyId)
        {
            string transactionNo = uniquecode;

            var module = await _moduleRepo.GetByCodeAsync(moduleCode);
            var user = await _userRepo.GetByIdAsync(userId);
            string content = $"{user.FirstName}, {actiontype}: {uniquecode}";
            string preview = $"{(content.Length > 50 ? $"{content.Substring(0, 50)}..." : content)}";
            string title = $"{module.Controller}";
            var receiver = await _roleAccessRepo.GetRoleByModuleCodeAsync(0, module.Code);

            var roleIds = receiver.Select(m => m.UserId).ToList();

            NotificationModel nModel = new()
            {
                SenderId = userId,
                PriorityLevel = 2, //important
                Title = title,
                Preview = preview,
                Content = content,
                ActionLink = actionlink,
                DateCreated = DateTime.Now,
                CompanyId = companyId,
                NotificationType = (int)NotificationType.Role,
                ProjectId = 0
            };

            var notification = await SaveNotificationAsync(nModel, roleIds, NotificationType.Role, userId);

            return notification;
        }

        public async Task UpdateNotificationStatusAsync(int id, bool isRead)
        {
            var notifToUpdate = await _context.NotificationReceivers.FirstOrDefaultAsync(x => x.Id == id);

            if (notifToUpdate == null)
                return;

            var userId = notifToUpdate.ReceiverId;
            notifToUpdate.IsRead = isRead;
            notifToUpdate.DateRead = DateTime.Now;

            await _notificationReceiverRepo.UpdateAsync(notifToUpdate, userId);
        }

        public async Task DeleteNotificationAsync(int id)
        {
            var notifToDelete = await _context.NotificationReceivers.FirstOrDefaultAsync(x => x.Id == id);

            if (notifToDelete == null)
                return;

            await _notificationReceiverRepo.DeleteAsync(notifToDelete.Id);
        }

        public async Task DeleteAllNotificationAsync(int userId, int companyId)
        {
            var notifIds = _context.Notifications.Where(n => n.CompanyId == companyId).Select(n => n.Id).ToList();

            var notifsToDelete = await _context.NotificationReceivers.Where(x => x.ReceiverId == userId && notifIds.Contains(x.NotifId)).Select(x => x.Id).ToListAsync();

            // Storing Ids in an int array
            int[] notifreceiverIds = notifsToDelete.ToArray();

            if (notifsToDelete == null) return;

            await _notificationReceiverRepo.BatchDeleteAsync(notifreceiverIds);
        }

        public async Task UpdateNotificationToTrashAsync(int id)
        {
            var notifToTrash = await _context.NotificationReceivers.FirstOrDefaultAsync(x => x.Id == id);
            var userId = notifToTrash.ReceiverId;

            if (notifToTrash == null)
                return;

            if (notifToTrash.IsDelete == false)
            {
                notifToTrash.IsDelete = true;
            }

            await _notificationReceiverRepo.UpdateAsync(notifToTrash, userId);
        }

        public async Task UpdateAllNotificationToTrashAsync(int userid)
        {
            var notifsToDelete = await _context.NotificationReceivers
                  .Where(x => x.IsDelete == false && x.ReceiverId == userid) // Filter rows where isDelete is 0
                  .ToListAsync();

            foreach (var notif in notifsToDelete)
            {
                notif.IsDelete = true;
                await _notificationReceiverRepo.UpdateAsync(notif, userid);
            }
        }

        public async Task<Notification> SaveAsync(Notification notification, int userId)
        {
            if (notification.Id == 0)
                notification = await CreateAsync(notification, userId);
            else
                notification = await UpdateAsync(notification, userId);

            return notification;
        }

        public async Task<Notification> CreateAsync(Notification notification, int userId)
        {
            notification.SenderId = userId;
            notification.DateCreated = DateTime.Now;

            var result = await _contextHelper.CreateAsync(notification);

            return result;
        }

        public async Task<Notification> UpdateAsync(Notification notification, int userId)
        {
            var result = await _contextHelper.UpdateAsync(notification, "DateCreated");

            return result;
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.Notifications.Where(v => ids.Contains(v.Id));

            await _contextHelper.BatchDeleteAsync(entities);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.Notifications.FirstOrDefault(v => id == v.Id);

            await _contextHelper.DeleteAsync(entities);
        }
    }
}