using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IBarrowersInformationRepository
    {
        Task<BarrowersInformation?> GetByIdAsync(int id);
        Task<BarrowersInformation?> GetByApplicationInfoIdAsync(int id);
        Task<BarrowersInformation> SaveAsync(BarrowersInformationModel model);
        Task<BarrowersInformation> CreateAsync(BarrowersInformationModel model);
        Task<BarrowersInformation> UpdateAsync(BarrowersInformationModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
