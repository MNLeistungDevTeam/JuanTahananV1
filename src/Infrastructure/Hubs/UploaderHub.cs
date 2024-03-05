using Microsoft.AspNetCore.SignalR;

namespace Template.Infrastructure.Hubs
{
    public class UploaderHub : Hub
    {
        public async Task ReceiveProgress(long total, int step, string element)
        {
            var progress = (step / (double)total) * 100;
            await Clients.All.SendAsync("ReceiveProgress", new
            {
                progress = progress,
                element = element
            });
        }
        public async Task UpdateSwalProgress(int progress)
        {
            await Clients.All.SendAsync("UpdateSwalProgress", progress);
        }
    }
}
