using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Entities;

namespace Template.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IApplicantsPersonalInformationRepository
    {
        Task<ApplicantsPersonalInformation?> GetByIdAsync(int id);
        Task<ApplicantsPersonalInformation?> GetbyUserId(int id);
        Task<List<ApplicantsPersonalInformation>?> GetAllAsync();
        Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model);
        Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformationModel model);
        Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformationModel model);
        Task DeleteAsync(int id);
        Task BatchDeleteAsync(int[] ids);
    }
}
