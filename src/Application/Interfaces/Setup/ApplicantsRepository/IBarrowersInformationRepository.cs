using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IBarrowersInformationRepository
    {
        Task<BarrowersInformation?> GetByIdAsync(int id);

        Task<BarrowersInformation?> GetByApplicationInfoIdAsync(int id);

        Task DeleteAsync(int id);

        Task BatchDeleteAsync(int[] ids);

        Task<BarrowersInformationModel?> GetByApplicantIdAsync(int applicantId);
        Task<BarrowersInformation> SaveAsync(BarrowersInformationModel model);
        Task<BarrowersInformation> CreateAsync(BarrowersInformation barrower);
        Task<BarrowersInformation> UpdateAsync(BarrowersInformation barrower);
    }
}