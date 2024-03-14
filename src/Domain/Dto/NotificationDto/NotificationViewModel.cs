using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.NotificationDto
{
    public class NotifReceiverViewModel
    {
        public NotificationModel? Notification { get; set; }
        public List<int> UserReceiver { get; set; } = new();
        public List<string> Receiver { get; set; } = new();
        public int Type { get; set; }
    }
}