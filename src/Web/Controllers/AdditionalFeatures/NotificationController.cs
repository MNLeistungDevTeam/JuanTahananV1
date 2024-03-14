using DMS.Application.Interfaces.Setup.NotificationRepo;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Domain.Dto.NotificationDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using DMS.Infrastructure.Hubs;
using DMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.AdditionalFeatures
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(
            INotificationRepository notificationRepo,
            IHubContext<NotificationHub> hubContext,
            IRoleRepository roleRepo)
        {
            _notificationRepo = notificationRepo;
            _hubContext = hubContext;
            _roleRepo = roleRepo;
        }

        #region View

        public IActionResult Index()
        {
            var viewmodel = new NotificationFilterModel();
            return View(viewmodel);
        }

        public IActionResult Create()
        {
            var viewmodel = new NotifReceiverViewModel();
            return View(viewmodel);
        }

        public async Task<IActionResult> Read(int id)
        {
            try
            {
                var data = await _notificationRepo.GetAsync(id);
                NotificationModel model = new();
                {
                    model.Id = data.Id;
                    model.Title = data.Title;
                    model.Content = data.Content;
                    model.DateCreated = data.DateCreated;
                }
                await _notificationRepo.UpdateNotificationStatusAsync(id, true);
                return View(model);
            }
            catch (Exception ex) { return View("Error", new ErrorViewModel { Message = ex.Message, Exception = ex }); }
        }

        #endregion View

        #region Get Methods

        public async Task<IActionResult> GetUnreadNotification(int pageNumber, int type)
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                int pageSize = 10;
                var data = await _notificationRepo.GetUnreadNotificationAsync(userId, pageNumber, pageSize, type, companyId);
                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> GetMyNotifications(NotificationFilterModel model)
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                model.UserId = userId;

                var data = await _notificationRepo.GetMyNotificationAsync(model, companyId);

                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> CountUnreadNotification()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                var data = await _notificationRepo.CountUnreadNotificationAsync(userId, companyId);

                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> CountUnreadTransaction()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                var data = await _notificationRepo.CountUnreadTransactionNotificationAsync(userId, companyId);

                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> CountUnreadApproval()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                var data = await _notificationRepo.CountUnreadApprovalNotificationAsync(userId, companyId);

                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> GetNotification(int id)
        {
            try
            {
                var data = await _notificationRepo.GetAsync(id);
                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> GetAnnouncementById(int id)
        {
            try
            {
                var data = await _notificationRepo.GetAnnouncementByIdAsync(id);
                return Ok(data);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        public async Task<IActionResult> GetRoles() =>
            Ok(await _roleRepo.GetAllRolesAsync());

        public async Task<IActionResult> GetAnnouncement() =>
            Ok(await _notificationRepo.GetAnnouncementAsync());

        #endregion Get Methods

        #region Action Methods

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotif(NotifReceiverViewModel vwmodel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Where(x => x.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }));

                int userId = int.Parse(User.Identity.Name);

                vwmodel.Notification.ActionLink = "";
                vwmodel.Notification.NotificationType = 1; //1 for Announcement Type

                //Save a Notification
                await _notificationRepo.SaveNotificationAsync(vwmodel.Notification, vwmodel.UserReceiver, (NotificationType)vwmodel.Type, userId);

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                };

                Notification notification = new();
                {
                    notification.Preview = vwmodel.Notification.Title + "  " + vwmodel.Notification.Preview;
                };

                //list
                List<int> receiverid = vwmodel.UserReceiver;

                for (int i = 0; i < receiverid.Count; i++)
                {
                    if (vwmodel.Type == (int)NotificationType.Role)
                    {
                        await _hubContext.Clients.Group(receiverid[i].ToString()).SendAsync("AddNotifGroup", notification, "eiFOS", serializerOptions);
                    }
                    else if (vwmodel.Type == (int)NotificationType.User)
                    {
                        await _hubContext.Clients.Group(receiverid[i].ToString()).SendAsync("AddNotifUser", notification, "eiFOS", serializerOptions);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Notification/UpdateNotifToReads")]
        public async Task<IActionResult> UpdateNotifToReads(string notifIds)
        {
            try
            {
                int[] Ids = Array.ConvertAll(notifIds.Split(','), int.Parse);

                if (Ids.Length > 0)
                {
                    foreach (var id in Ids)
                    {
                        await _notificationRepo.UpdateNotificationStatusAsync(id, true);
                    }
                }

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("Notification/UpdateNotifToUnReads")]
        public async Task<IActionResult> UpdateNotifToUnReads(string notifIds)
        {
            try
            {
                int[] Ids = Array.ConvertAll(notifIds.Split(','), int.Parse);

                if (Ids.Length > 0)
                {
                    foreach (var id in Ids)
                    {
                        await _notificationRepo.UpdateNotificationStatusAsync(id, false);
                    }
                }

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("Notification/ArchiveAllNotification")]
        public async Task<IActionResult> ArchiveAllNotification()
        {
            try
            {
                var userid = int.Parse(User.Identity.Name);
                await _notificationRepo.UpdateAllNotificationToTrashAsync(userid);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]
        [Route("Notification/ArchiveNotification")]
        public async Task<IActionResult> ArchiveNotification(string notifIds)
        {
            try
            {
                int[] Ids = Array.ConvertAll(notifIds.Split(','), int.Parse);

                if (Ids.Length > 0)
                    foreach (var id in Ids)
                        await _notificationRepo.UpdateNotificationToTrashAsync(id);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        #region Hard Delete

        [HttpDelete]
        [Route("Notification/DeleteNotification")]
        public async Task<IActionResult> DeleteNotification(string notifIds)
        {
            try
            {
                int[] Ids = Array.ConvertAll(notifIds.Split(','), int.Parse);

                if (Ids.Length > 0)
                    foreach (var id in Ids)
                        await _notificationRepo.DeleteNotificationAsync(id);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete]
        [Route("Notification/DeleteAllNotifications")]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            try
            {
                int userId = int.Parse(User.Identity.Name);
                int companyId = int.Parse(User.FindFirstValue("Company"));
                await _notificationRepo.DeleteAllNotificationAsync(userId, companyId);

                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        #endregion Hard Delete

        #endregion Action Methods
    }
}