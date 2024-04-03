using DMS.Domain.Dto.BeneficiaryInformationDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.BeneficiaryInformationRepo
{
    public interface IBeneficiaryInformationRepository
    {
        Task BachDeleteAsync(int[] ids);
        Task<BeneficiaryInformation> CreateAsync(BeneficiaryInformation beneficiaryInformation, int userId);
        Task<List<BeneficiaryInformation>> GetAllAsync();
        Task<BeneficiaryInformation?> GetByIdAsync(int id);
        Task<BeneficiaryInformationModel?> GetByPagibigNumberAsync(string? pagibigNumber);
        Task<BeneficiaryInformation> SaveAsync(BeneficiaryInformationModel beneficiaryInformation, int userId);
        Task<BeneficiaryInformation> UpdateAsync(BeneficiaryInformation beneficiaryInformation, int userId);
    }
}
