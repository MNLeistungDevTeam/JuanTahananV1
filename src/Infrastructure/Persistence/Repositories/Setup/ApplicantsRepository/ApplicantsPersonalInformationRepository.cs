using AutoMapper;
using DMS.Domain.Dto.ApplicantsDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Entities;
using DevExpress.CodeParser;

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

        public async Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<ApplicantsPersonalInformationModel>(await CreateAsync(model));
            else
                model = _mapper.Map<ApplicantsPersonalInformationModel>(await UpdateAsync(model));
            return _mapper.Map<ApplicantsPersonalInformation>(model);
        }

        public async Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformationModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<ApplicantsPersonalInformation>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }

        public async Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformationModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<ApplicantsPersonalInformation>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated", "CreatedById");
            return mapped;
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