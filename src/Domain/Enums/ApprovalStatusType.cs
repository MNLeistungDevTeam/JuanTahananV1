﻿using System;
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

    public enum AppStatusType
    {
        Draft = 0,
        Submitted = 1,
        Deferred = 2,
        DeveloperVerified = 3,
        PagibigVerified = 4,
        Withdrawn = 5,
        PostSubmitted = 6,
        DeveloperConfirmed = 7,
        PagibigConfirmed = 8,
        Disqualified = 9,
        Discontinued = 10              //Postwithdrawn
    }
}
