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
        Task<IEnumerable<PropertyLocationModel>> GetLocationsByProject(int? projectId, int? developerId);
        Task<IEnumerable<PropertyProjectModel>> GetProjectsByCompany(int companyId, int? locationId);
        Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model, string? rootFolder);
    }
}