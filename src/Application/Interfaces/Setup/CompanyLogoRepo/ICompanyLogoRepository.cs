using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.CompanyLogoRepo
{
    public interface ICompanyLogoRepository
    {
        Task BatchDeleteAsync(CompanyLogo[] module);
        Task<CompanyLogo> CreateAsync(CompanyLogo module, int userId, params string[] excludes);
        Task<CompanyLogoModel> GetByDescAsync(int companyId, string description);
        Task<IEnumerable<CompanyLogoModel>> GetByCompanyIdAsync(int companyId);
        Task<CompanyLogo> UpdateAsync(CompanyLogo module, int userId, params string[] excludes);
        Task<List<CompanyLogo>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<CompanyLogo> SaveAsync(CompanyLogo model, int userId);
        Task<CompanyLogo?> GetByIdAsync(int id);
    }
}
