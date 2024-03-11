using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.NotificationReceiverDto
{
    public class NotificationReceiverModel
    {

        public int Id { get; set; }

        public int NotifId { get; set; }

        [Required]
        [Display(Name = "Recipient Name")]
        public int ReceiverId { get; set; }

        [Required]
        [Display(Name = "Recipient Type")]
        public int ReceiverType { get; set; }

        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }

        public bool IsDelete { get; set; }

        #region Display Properties

        public string? Title { get; set; }

        public string? Content { get; set; }
        public string? Preview { get; set; }
        public string? ActionLink { get; set; }

        public int PriorityLevel { get; set; }
        public string? LevelName { get; set; }
        public DateTime? DateCreated { get; set; }
        public int SenderId { get; set; }
        public string? SenderName { get; set; }

        public string? ReceiverTypeName { get; set; }
        public int UnreadNotif { get; set; }

        public int PageNumberLimit { get; set; }

        #endregion
    }
}
