using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Enums
{
    public enum ApprovalStatusType
    {
        Pending = 0,
        PendingReview = 1,
        Approved = 2,
        Disapproved = 3,
        Returned = 4,
        Closed = 5,
        Cancelled = 6,
    }
}
