﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class CompanyLogo
{
    public int Id { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public bool IsDisabled { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }

    public int? CompanyId { get; set; }

    public virtual Company Company { get; set; }
}