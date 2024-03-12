using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.CompanySettingRepo
{
    public interface ICompanySettingRepository
    {
        Task BatchDeleteAsync(int[] ids);
        Task<CompanySetting> CreateAsync(CompanySetting module, int userId);
        Task DeleteAsync(int id);
        Task<List<CompanySetting>> GetAllAsync();
        Task<CompanySetting?> GetByCompanyIdAsync(int companyId);
        Task<CompanySetting?> GetByIdAsync(int id);
        Task SaveAsync(CompanySetting model, int userId);
        Task<CompanySetting> UpdateAsync(CompanySetting module, int userId);
    }
}
