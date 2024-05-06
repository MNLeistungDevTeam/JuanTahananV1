﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PropertyManagementDto;

public class PropertyProjectLocationModel
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int LocationId { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }
}