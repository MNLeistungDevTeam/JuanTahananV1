﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DMS.Domain.Entities;

public partial class ApplicantsPersonalInformation
{
    public int Id { get; set; }

    public string Code { get; set; }

    public int UserId { get; set; }

    public long? PagibigNumber { get; set; }

    public long? HousingAccountNumber { get; set; }

    public DateTime DateCreated { get; set; }

    public int CreatedById { get; set; }

    public DateTime? DateModified { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateDeleted { get; set; }

    public int? DeletedById { get; set; }
}