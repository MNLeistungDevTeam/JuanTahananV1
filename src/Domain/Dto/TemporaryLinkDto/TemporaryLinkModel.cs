using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.TemporaryLinkDto
{
    public class TemporaryLinkModel
    {
        public int Id { get; set; }

        public Guid GuId { get; set; }

        public int? UserId { get; set; }

      
        public bool? IsSubmitted { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public bool? IsOpened { get; set; }

        public DateTime? DateExpiration { get; set; }
    }
}
