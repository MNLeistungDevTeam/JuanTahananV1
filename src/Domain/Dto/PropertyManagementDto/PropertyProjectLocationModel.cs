using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PropertyManagementDto;

public class PropertyProjectLocationModel
{
    public int Id { get; set; }

    [Display(Name ="Project")]
    public int ProjectId { get; set; }


    [Display(Name = "Location")]
    public int LocationId { get; set; }

    public int CreatedById { get; set; }

    public DateTime DateCreated { get; set; }

    public int? ModifiedById { get; set; }

    public DateTime? DateModified { get; set; }

    public string? ProjectName { get; set; }
    public string? LocationName { get; set; }
}