using DMS.Domain.Dto.DocumentVerificationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.DocumentVerification
{
    public interface IDocumentVerificationRepository
    {
        Task BatchDeleteAsync(int[] ids);

        Task DeleteAsync(int id);
        Task<DocumentVerificationModel?> GetByDocumentTypeId(int id);
        Task<IEnumerable<DocumentVerificationModel?>> GetByTypeAsync(int type, string? applicantCode);

        Task<DMS.Domain.Entities.DocumentVerification> SaveAsync(DocumentVerificationModel model, int userId);
    }
}