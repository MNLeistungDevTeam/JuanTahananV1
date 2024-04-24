using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Interfaces.Setup.ApprovalStatusRepo;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Dto.ApprovalStatusDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class ApplicantsPersonalInformationRepository : IApplicantsPersonalInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ApplicantsPersonalInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly IModuleRepository _moduleRepo;
        private readonly IApprovalStatusRepository _approvalStatusRepo;

        public ApplicantsPersonalInformationRepository(DMSDBContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ISQLDatabaseService db,
            IModuleRepository moduleRepo,
            IApprovalStatusRepository approvalStatusRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ApplicantsPersonalInformation>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
            _moduleRepo = moduleRepo;
            _approvalStatusRepo = approvalStatusRepo;
        }

        #region Get Methods

        public async Task<ApplicantsPersonalInformation?> GetByIdAsync(int id) =>
            await _context.ApplicantsPersonalInformations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ApplicantsPersonalInformation?> GetbyUserId(int id) =>
            await _context.ApplicantsPersonalInformations.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id);

        public async Task<List<ApplicantsPersonalInformation>?> GetAllAsync() =>
            await _context.ApplicantsPersonalInformations.AsNoTracking().ToListAsync();

        public async Task<ApplicantsPersonalInformationModel?> GetAsync(int id) =>
         await _db.LoadSingleAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_Get", new { id });

        public async Task<ApplicantsPersonalInformationModel?> GetByCodeAsync(string code) =>
          await _db.LoadSingleAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetByCode", new { code });

        public async Task<ApplicantsPersonalInformationModel?> GetCurrentApplicationByUser(int userId, int companyId) =>
          await _db.LoadSingleAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetByUserId", new { userId, companyId });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel>?> GetApplicationTimelineByCode(string code, int companyId) =>
          await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetApplicationTimelineByCode", new { code, companyId });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel?>> GetApplicantsAsync(int? roleId, int? companyId) =>
          await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetAll", new { roleId, companyId });

        public async Task<IEnumerable<ApprovalInfoModel>> GetApprovalTotalInfo(int? userId, int companyId) =>
           await _db.LoadDataAsync<ApprovalInfoModel, dynamic>("spApplicantsPersonalInformation_GetTotalInfo", new { userId, companyId });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel>> GetAllApplicationsByPagibigNumber(string? pagibigNumber) =>
            await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetAllByPagibigNumber", new { pagibigNumber });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel>> GetEligibilityVerificationDocuments(string applicantCode) =>
             await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetEligibilityVerificationDocuments", new { applicantCode });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel>> GetApplicationVerificationDocuments(string applicantCode) =>
           await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetApplicationVerificationDocuments", new { applicantCode });

        public async Task<ApplicationInfoModel?> GetApplicationInfo(int roleId, string pagibigNumber) =>
            await _db.LoadSingleAsync<ApplicationInfoModel, dynamic>("spApplicantsPersonalInformation_GetInfo", new { roleId, pagibigNumber });

        public async Task<ApplicationInfoModel?> GetTotalApplication(int roleId, int companyId) =>
            await _db.LoadSingleAsync<ApplicationInfoModel, dynamic>("spApplicantsPersonalInformation_GetTotalApplication", new { roleId, companyId });

        public async Task<ApplicationInfoModel?> GetTotalCreditVerif(int companyId) =>
            await _db.LoadSingleAsync<ApplicationInfoModel, dynamic>("spApplicantsPersonalInformation_GetTotalCreditVerif", new { companyId });

        public async Task<ApplicationInfoModel?> GetTotalAppVerif(int companyId) =>
            await _db.LoadSingleAsync<ApplicationInfoModel, dynamic>("spApplicantsPersonalInformation_GetTotalAppVerif", new { companyId });

        #endregion Get Methods

        #region Api

        public async Task<ApplicantsPersonalInformation> UpdateNoExclusionAsync(ApplicantsPersonalInformation applicant, int updatedById)
        {
            try
            {
                applicant.ModifiedById = updatedById;
                applicant.DateModified = DateTime.Now;
                applicant = await _contextHelper.UpdateAsync(applicant, "ApprovalStatus", "DateCreated", "ModifiedById");

                return applicant;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model, int userId)
        {
            var _applicantPersonalInfo = _mapper.Map<ApplicantsPersonalInformation>(model);

            if (!string.IsNullOrEmpty(_applicantPersonalInfo.PagibigNumber))
            {
                if (_applicantPersonalInfo.PagibigNumber.Length < 12)
                {
                    throw new Exception("Pagibig Number minimum length must 12");
                }
            }

            if (model.Id == 0)
            {
                if (_applicantPersonalInfo.ApprovalStatus is null)
                {
                    _applicantPersonalInfo.ApprovalStatus = (int)AppStatusType.Draft;
                }

                _applicantPersonalInfo.Code = await GenerateApplicationCode();

                _applicantPersonalInfo = await CreateAsync(_applicantPersonalInfo, userId);

                int? intValue = _applicantPersonalInfo.ApprovalStatus;
                AppStatusType enumValue = (AppStatusType)intValue;

                // Create Initial Approval Status
                await _approvalStatusRepo.CreateInitialApprovalStatusAsync(_applicantPersonalInfo.Id, ModuleCodes2.CONST_APPLICANTSREQUESTS, userId, _applicantPersonalInfo.CompanyId.Value, enumValue);
            }
            else
            {
                var applicationStatus = await GetByCodeAsync(_applicantPersonalInfo.Code);

                _applicantPersonalInfo.ApprovalStatus = applicationStatus.ApprovalStatus;

                if (_applicantPersonalInfo.EncodedStatus != null)
                {
                    _applicantPersonalInfo.ApprovalStatus = _applicantPersonalInfo.EncodedStatus;
                }

                //approvalstatus must not update
                //_applicantPersonalInfo = await UpdateNoExclusionAsync(_applicantPersonalInfo, userId);

                await UpdateAsync(_applicantPersonalInfo, userId);

                var moduleStage = await _moduleRepo.GetByCodeAsync(ModuleCodes2.CONST_APPLICANTSREQUESTS);
                var approvalStatus = await _approvalStatusRepo.GetByReferenceIdAsync(_applicantPersonalInfo.Id, _applicantPersonalInfo.CompanyId);

                if (approvalStatus == null)
                {
                    if (moduleStage is not null && moduleStage.WithApprover)
                    {
                        // Create Initial Approval Status
                        await _approvalStatusRepo.CreateInitialApprovalStatusAsync(_applicantPersonalInfo.Id, ModuleCodes2.CONST_APPLICANTSREQUESTS, userId, _applicantPersonalInfo.CompanyId.Value);
                    }
                }
            }

            return _applicantPersonalInfo;
        }

        public async Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId)
        {
            applicantPersonalInfo.DateCreated = DateTime.Now;
            applicantPersonalInfo.CreatedById = userId;

            applicantPersonalInfo = await _contextHelper.CreateAsync(applicantPersonalInfo, "DateModified", "ModifiedById");
            return applicantPersonalInfo;
        }

        public async Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformation applicantPersonalInfo, int userId)
        {
            applicantPersonalInfo.DateModified = DateTime.Now;
            applicantPersonalInfo.ModifiedById = _currentUserService.GetCurrentUserId();
            applicantPersonalInfo = await _contextHelper.UpdateAsync(applicantPersonalInfo, "DateCreated", "CreatedById");
            return applicantPersonalInfo;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
            {
                entity.DateDeleted = DateTime.Now;
                entity.DeletedById = _currentUserService.GetCurrentUserId();
                if (entity is not null)
                    await _contextHelper.UpdateAsync(entity);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.ApplicantsPersonalInformations.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }

        public async Task<string> GenerateApplicationCode()
        {
            try
            {
                string newref = $"APL{DateTime.Now:yyyyMM}-{"1".ToString().PadLeft(4, '0')}";
                var result = await _db.LoadSingleAsync<string, dynamic>("spApplicantsPersonalInformation_GenerateCode", new { });

                if (result != null)
                {
                    newref = $"APL{DateTime.Now:yyyyMM}-{(Convert.ToInt32(result.Remove(0, result.Length - 4)) + 1).ToString().PadLeft(4, '0')}";
                }

                return newref;
            }
            catch (Exception) { throw; }
        }
    }

    #endregion Api
}