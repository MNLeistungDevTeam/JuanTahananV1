using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface ISpouseRepository
    {
        Task<Spouse?> GetByIdAsync(int id);
        Task<Spouse> SaveAsync(SpouseModel model);
        Task<Spouse?> GetByApplicationInfoIdAsync(int id);
        Task<Spouse> CreateAsync(SpouseModel model);
        Task<Spouse> UpdateAsync(SpouseModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
