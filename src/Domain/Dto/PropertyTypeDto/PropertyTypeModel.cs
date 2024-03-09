using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PropertyTypeDto
{
    public class PropertyTypeModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }
    }
}
