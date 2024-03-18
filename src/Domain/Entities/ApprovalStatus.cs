﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class ApprovalStatus
{
    public int Id { get; set; }

    /// <summary>
    /// Prepared By
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Record Id
    /// </summary>
    public int ReferenceId { get; set; }

    /// <summary>
    /// Module Id
    /// </summary>
    public int ReferenceType { get; set; }

    /// <summary>
    /// 0 = For Approval, 1 = Approved, 2 = Canceled
    /// </summary>
    public int Status { get; set; }

    public DateTime LastUpdate { get; set; }
}