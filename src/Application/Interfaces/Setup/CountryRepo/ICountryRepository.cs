using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.CountryRepo
{
    public interface ICountryRepository
    {
        Task BatchDeleteAsync(int[] ids);
        Task<Country> CreateAsync(Country module, int userId);
        Task DeleteAsync(int id);
        Task<IEnumerable<CountryModel>> GetAllAsync();
        Task<CountryModel?> GetByIdAsync(int id);
        Task SaveAsync(Country model, int userId);
        Task<Country> UpdateAsync(Country module, int userId);
    }
}
