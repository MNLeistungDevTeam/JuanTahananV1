using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PropertyManagementDto;

public class PropertyUnitModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }

    public string? ProfileImage { get; set; } = string.Empty;
    public IFormFile? ProfileImageFile { get; set; }
}