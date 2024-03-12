using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.NotificationDto
{
    public class NotificationFilterModel
    {
        public int? UserId { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDelete { get; set; }
    }
}
