using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.BuyerConfirmationRepo
{
    public interface IBuyerConfirmationRepository
    {
        Task<BuyerConfirmationModel?> GetByCodeAsync(string code);
        Task<BuyerConfirmationModel?> GetAsync(int id);
        Task<BuyerConfirmationModel?> GetByUserAsync(int userId);
        Task<BuyerConfirmation> SaveAsync(BuyerConfirmationModel bcModel, int userId);
    }
}
