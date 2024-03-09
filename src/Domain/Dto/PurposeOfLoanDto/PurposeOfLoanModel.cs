using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.PurposeOfLoanDto
{
    public class PurposeOfLoanModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public int CreatedById { get; set; }
    }
}
