using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.OtherDto
{
    public class DropdownModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
    }
}
