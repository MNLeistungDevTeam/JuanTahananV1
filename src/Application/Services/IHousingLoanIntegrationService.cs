using DMS.Domain.Dto.BasicBeneficiaryDto;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Dto.PropertyManagementDto;
using System;
using System.Linq;

namespace DMS.Application.Services
{
    public interface IHousingLoanIntegrationService
    {
        Task<IEnumerable<CompanyModel>> GetDevelopers();
        Task<PropertyProjectModel> GetProjectsByCompany(int companyId);
        Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model, string? rootFolder);
    }
}