﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Template.Domain.Entities;

public partial class RoleAccess
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ModuleId { get; set; }

    public bool CanCreate { get; set; }

    public bool CanModify { get; set; }

    public bool CanDelete { get; set; }

    public bool CanRead { get; set; }

    public bool FullAccess { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }
}