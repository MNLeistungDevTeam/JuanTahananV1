using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.EmailLogDto
{
    public class EmailLogModel
    {
        public int Id { get; set; }

        public int ReferenceId { get; set; }

        public string ReferenceNo { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public DateTime? Date { get; set; }

        #region Display Properties

        public string? ReferenceNos { get; set; }
        public string? ReferenceIds { get; set; }
        public string? MailStatus { get; set; }
        public string? VendorName { get; set; }
        public string? VendorCode { get; set; }
        public string? ReceiverName { get; set; }
        public string? SenderName { get; set; }

        #endregion Display Properties
    }
}
