using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Dto.BuyerConfirmationDocumentDto
{
    public class BuyerConfirmationDocumentModel
    {
        public int Id { get; set; }

        public int? ReferenceId { get; set; }

        public string ReferenceNo { get; set; }

        public int? Status { get; set; }

        public string Remarks { get; set; }

        public int? CreatedById { get; set; }

        public DateTime? DateCreated { get; set; }

        public int? ModifiedById { get; set; }

        public DateTime? DateModified { get; set; }
        public int? CompanyId { get; set; }
    }
}
