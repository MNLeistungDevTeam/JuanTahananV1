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

        #region Get

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

        public async Task<ApplicantsPersonalInformationModel?> GetByUserAsync(int userId) =>
          await _db.LoadSingleAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetByUserId", new { userId });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel?>> GetApplicantsAsync() =>
          await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetAll", new { });

        public async Task<IEnumerable<ApprovalInfoModel>> GetApprovalTotalInfo(int? userId) =>
           await _db.LoadDataAsync<ApprovalInfoModel, dynamic>("spApplicantsPersonalInformation_GetTotalInfo", new { userId });

        public async Task<IEnumerable<ApplicantsPersonalInformationModel>> GetEligibilityVerificationDocuments(string applicantCode) =>
             await _db.LoadDataAsync<ApplicantsPersonalInformationModel, dynamic>("spApplicantsPersonalInformation_GetEligibilityVerficationDocuments", new { applicantCode });

        #endregion Get

        #region Api

        public async Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model, int userId)
        {
            var _applicantPersonalInfo = _mapper.Map<ApplicantsPersonalInformation>(model);

            _applicantPersonalInfo.ApprovalStatus = (int)AppStatusType.Draft;

            if (model.Id == 0)
            {
                _applicantPersonalInfo = await CreateAsync(_applicantPersonalInfo, userId);

                // Create Initial Approval Status
                await _approvalStatusRepo.CreateInitialApprovalStatusAsync(_applicantPersonalInfo.Id, ModuleCodes2.CONST_APPLICANTSREQUESTS, userId, _applicantPersonalInfo.CompanyId.Value);
            }
            else
            {
                _applicantPersonalInfo = await UpdateAsync(_applicantPersonalInfo, userId);

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
    }

    #endregion Api
}