using DMS.Domain.Dto.ApplicantsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Domain.Entities;
using DMS.Domain.Dto.ApprovalStatusDto;

namespace DMS.Application.Interfaces.Setup.ApplicantsRepository
{
    public interface IApplicantsPersonalInformationRepository
    {
        Task<string> GenerateApplicationCode();
        Task<IEnumerable<ApprovalInfoModel>> GetApprovalTotalInfo(int? userId);
        Task<ApplicantsPersonalInformation?> GetByIdAsync(int id);

        Task<ApplicantsPersonalInformation?> GetbyUserId(int id);

        Task<List<ApplicantsPersonalInformation>?> GetAllAsync();

        Task DeleteAsync(int id);

        Task BatchDeleteAsync(int[] ids);

        Task<ApplicantsPersonalInformationModel?> GetByCodeAsync(string code);

        Task<ApplicantsPersonalInformationModel?> GetByUserAsync(int userId);

        Task<ApplicantsPersonalInformationModel?> GetAsync(int id);
        Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model, int userId);
        Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId);
        Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId);
        Task<IEnumerable<ApplicantsPersonalInformationModel?>> GetApplicantsAsync(int? roleId);
        Task<IEnumerable<ApplicantsPersonalInformationModel>> GetEligibilityVerificationDocuments(string applicantCode);
        Task<IEnumerable<ApplicantsPersonalInformationModel>> GetAllApplicationsByPagibigNumber(string? pagibigNumber);
        Task<IEnumerable<ApplicantsPersonalInformationModel>> GetApplicationVerificationDocuments(string applicantCode);
        Task<ApplicationInfoModel?> GetApplicationInfo(int roleId);
    }
}