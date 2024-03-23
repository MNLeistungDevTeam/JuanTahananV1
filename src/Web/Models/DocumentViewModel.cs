
using DMS.Domain.Dto.DocumentDto;
using DMS.Domain.Dto.DocumentVerificationDto;
using DMS.Domain.Entities;

namespace DMS.Web.Models
{
    public class DocumentViewModel
    {
        public DocumentTypeModel? DocumentType { get; set; }
        public DocumentVerificationModel? DocumentVerification { get; set; }
    }
}
