﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class ApprovalLog
{
    public int Id { get; set; }

    /// <summary>
    /// Transaction Record Id
    /// </summary>
    public int ReferenceId { get; set; }

    /// <summary>
    /// Module Stage Id
    /// </summary>
    public int StageId { get; set; }

    /// <summary>
    /// 1 = Approved, 2 = Rejected, 3 = Cancelled
    /// </summary>
    public int Action { get; set; }

    public string Comment { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }

    public int? ApprovalLevelId { get; set; }

    public int? ApprovalStatusId { get; set; }
}