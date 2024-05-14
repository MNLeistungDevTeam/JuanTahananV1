using DMS.Domain.Dto.BuyerConfirmationDocumentDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.BuyerConfirmationDocumentRepo
{
    public interface IBuyerConfirmationDocumentRepository
    {
        Task<BuyerConfirmationDocument?> GetByIdAsync(int id);
        Task<BuyerConfirmationDocument?> GetByReferenceIdAsync(int documentId);
        Task<BuyerConfirmationDocument> SaveAsync(BuyerConfirmationDocumentModel bcModel, int userId);
        Task<BuyerConfirmationDocument> UpdateAsync(BuyerConfirmationDocument buyerConfirm, int userId);
    }
}
