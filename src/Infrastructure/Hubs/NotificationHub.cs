using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Hubs
{

    public class NotificationHub : Hub
    {
        public async Task SendMessage(string groupName, string user, string message) =>
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);

        public async Task UpdateAllUser(string groupName, string userName, string message) =>
            await Clients.Group(groupName).SendAsync("AddNotifToPage", userName, message);

        public async Task UpdateNotifGroup(string groupName, string companyCode, string message) =>
            await Clients.Group(groupName).SendAsync("AddNotifGroup", companyCode, message);

        public async Task GetServerTime() =>
            await Clients.All.SendAsync("RetServerTime", DateTime.Now);

        public void JoinGroup(string groupName) =>
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        public async Task UpdateNotifUser(string userId, string message) =>
            await Clients.User(userId).SendAsync("AddNotifToUser", message);
    }
}
