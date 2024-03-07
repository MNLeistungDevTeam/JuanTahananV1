using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
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

        public ApplicantsPersonalInformationRepository(
            DMSDBContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ISQLDatabaseService db)
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

        public async Task<ApplicantsPersonalInformation> SaveAsync(ApplicantsPersonalInformationModel model)
        {
            var _model = _mapper.Map<ApplicantsPersonalInformation>(model);

            if (_model.Id == 0)
                _model = await CreateAsync(_model);
            else
                _model = await UpdateAsync(_model);

            return _model;
        }

        public async Task<ApplicantsPersonalInformation> CreateAsync(ApplicantsPersonalInformation model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId(); 

            model = await _contextHelper.CreateAsync(model, "DateModified", "ModifiedById");
            return model;
        }

        public async Task<ApplicantsPersonalInformation> UpdateAsync(ApplicantsPersonalInformation model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();

            model = await _contextHelper.UpdateAsync(model, "DateCreated", "CreatedById");
            return model;
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