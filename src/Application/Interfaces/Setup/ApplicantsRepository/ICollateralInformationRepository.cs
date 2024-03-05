using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface ICollateralInformationRepository
    {
        Task<CollateralInformation?> GetByIdAsync(int id);
        Task<CollateralInformation?> GetByApplicationInfoIdAsync(int id);
        Task<CollateralInformation> SaveAsync(CollateralInformationModel model);
        Task<CollateralInformation> CreateAsync(CollateralInformationModel model);
        Task<CollateralInformation> UpdateAsync(CollateralInformationModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
