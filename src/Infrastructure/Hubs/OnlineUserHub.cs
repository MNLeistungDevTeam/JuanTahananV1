using DMS.Application.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Hubs
{


    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new();
    }

    public class OnlineUserHub : Hub
    {
        private readonly IAuthenticationService _authenticationService;

        public OnlineUserHub(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);

            var user = Context.User;
            int userId = 0;

            if (user is not null && user.Identity.IsAuthenticated)
            {
                userId = int.Parse(user.Identity.Name);
            }

            await _authenticationService.UpdateOnlineStatus(userId, true);

            await base.OnConnectedAsync();
            await Clients.All.SendAsync("UpdateOnlineUser", UserHandler.ConnectedIds.Count);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);

            var user = Context.User;
            int userId = 0;

            if (user is not null && user.Identity.IsAuthenticated)
            {
                userId = int.Parse(user.Identity.Name);
            }

            await _authenticationService.UpdateOnlineStatus(userId, false);

            await base.OnDisconnectedAsync(exception);
            await Clients.All.SendAsync("UpdateOnlineUser", UserHandler.ConnectedIds.Count);
        }
    }
}
