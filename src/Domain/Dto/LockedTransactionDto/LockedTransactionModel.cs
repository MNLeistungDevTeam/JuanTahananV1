using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.LockedTransactionDto
{
    public class LockedTransactionModel
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public string TransactionNo { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
