using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.CompanyRepo
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync2(int companyId);
        Task DeleteAsync(int id);
        Task<IEnumerable<CompanyModel>> GetAllAsync();
        Task<CompanyModel?> GetByIdAsync(int id);
        Task<Company?> GetByCodeAsync(string code);
        Task<CompanyInfoModel> GetInfoAsync(int companyId);
        Task SaveAsync(Company model, List<CompanyLogo> clModel, List<Address> aModel, int userId, string webHostingEnv, CompanySetting csModel);
        Task<Company> CreateAsync(Company module, int userId);
        Task<Company> UpdateAsync(Company module, int userId);
        Task BatchDeleteAsync(int[] ids);
    }
}
