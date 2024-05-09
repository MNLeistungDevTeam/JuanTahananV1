using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PropertyManagementDto;

public class PropertyProjectModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string? Logo { get; set; } = string.Empty;

    [Display(Name = "Company")]
    public int? CompanyId { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }

    public string? CompanyName { get; set; }


    public int LocationId { get; set; }
}



