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
        Task<CompanyLogo> SaveAsync(CompanyLogo companyLogo, int userId);

        Task<IEnumerable<CompanyLogoModel>> GetByCompanyId(int companyId);

        Task<List<CompanyLogo>> GetByCompanyIdAsync(int id);

        Task BatchDeleteAsync(List<CompanyLogo> companyLogos);

        Task<CompanyLogoModel?> GetByDesc(int companyId, string description);

        Task BatchDeleteAsync(int[] ids);

        Task<CompanyLogo> CreateAsync(CompanyLogo model, int userId);

        Task DeleteAsync(int id);

        Task<List<CompanyLogo>> GetAllAsync();

        Task<CompanyLogo?> GetByIdAsync(int id);

        Task<CompanyLogo> UpdateAsync(CompanyLogo model, int userId);
    }
}