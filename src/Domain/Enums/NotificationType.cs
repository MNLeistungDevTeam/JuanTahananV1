using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Enums
{
    public enum NotificationType
    {
        User = 1,
        Role = 2,
        Approval = 3,
        Project = 4       // filter by RoleAccess and Project    
    }
}
