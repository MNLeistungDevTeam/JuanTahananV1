using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.NotificationDto
{
    public class NotificationModel
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        [Required]
        public string? Preview { get; set; }

        [Display(Name = "Action Link")]
        public string? ActionLink { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateCreated { get; set; }

        public int SenderId { get; set; }

        public int NotificationType { get; set; }
        public int ProjectId { get; set; }

        [Required]
        public int PriorityLevel { get; set; }

        public string? SenderName { get; set; }
        public string? ReceiverName { get; set; }

        public string? LevelName { get; set; }

        public int ReceiverId { get; set; }
        public int CompanyId { get; set; }
        public int ReceiverType { get; set; }
        public string? ReceiverTypeName { get; set; }
        public string? Fullname { get; set; }
    }
}