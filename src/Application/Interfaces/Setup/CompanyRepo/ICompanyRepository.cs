using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.EntityDto;
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
        Task BatchDeleteAsync(int[] ids);

        Task<Company> CreateAsync(Company company, int createdById);

        Task DeleteAsync(int id);

        Task<List<Company>> GetAllAsync();

        Task<Company?> GetByIdAsync(int id);

        Task<IEnumerable<CompanyModel>> GetCompanies();

        Task<CompanyModel?> GetCompany(int id);

        Task<Company?> GetCompanyByCode(string code);

        Task<CompanyInfoModel?> GetCompanyInfo(int companyId);

        Task SaveAsync(CompanyModel model, List<CompanyLogoModel> clModel, List<AddressModel> aModel, CompanySettingModel csModel, int userId, string webHostingEnv);

        Task<Company> UpdateAsync(Company company, int modifiedById);
    }
}
