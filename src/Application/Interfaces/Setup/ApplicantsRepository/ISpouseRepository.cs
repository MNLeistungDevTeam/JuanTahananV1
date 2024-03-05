using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.ApplicantsRepository
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
