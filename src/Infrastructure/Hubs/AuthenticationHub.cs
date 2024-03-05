using Microsoft.AspNetCore.SignalR;

namespace DMS.Infrastructure.Hubs;

public class AuthenticationHub : Hub
{
    public async Task CheckIfAuthenticated()
    {
        var identity = Context.User?.Identity;
        var isAuthenticated = identity is not null && identity.IsAuthenticated;
        await Clients.All.SendAsync("IsAuthenticated", isAuthenticated);
    }
}