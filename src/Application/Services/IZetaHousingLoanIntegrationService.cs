using DMS.Domain.Dto.ZetaHousingLoanModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Services
{
    public interface IZetaHousingLoanIntegrationService
    {
        Task SaveBeneficiaryAsync(BasicBeneficiaryInformationModel model);
    }
}