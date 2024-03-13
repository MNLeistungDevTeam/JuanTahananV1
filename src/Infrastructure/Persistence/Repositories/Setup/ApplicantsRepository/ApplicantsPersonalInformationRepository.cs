using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class ApplicantsPersonalInformationRepository : IApplicantsPersonalInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ApplicantsPersonalInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public ApplicantsPersonalInformationRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ApplicantsPersonalInformation>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

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

        public async Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model, int userId)
        {
            var _applicantPersonalInfo = _mapper.Map<ApplicantsPersonalInformation>(model);

            if (model.Id == 0)
                _applicantPersonalInfo = await CreateAsync(_applicantPersonalInfo, userId);
            else
                _applicantPersonalInfo = await UpdateAsync(_applicantPersonalInfo, userId);
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
                entity.DateDeleted = DateTime.UtcNow;
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
}