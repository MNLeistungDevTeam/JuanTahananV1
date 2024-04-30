using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.ReferenceDto
{
    public class ReferenceModel
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public int ReceiverId { get; set; }
        public int SenderId { get; set; }
        public string Description { get; set; }

    }
}
