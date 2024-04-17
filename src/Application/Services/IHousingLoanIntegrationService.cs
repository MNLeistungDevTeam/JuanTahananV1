using DMS.Domain.Dto.BasicBeneficiaryDto;
using System;
using System.Linq;

namespace DMS.Application.Services
{
    public interface IHousingLoanIntegrationService
    {
        Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model, string? rootFolder);
    }
}