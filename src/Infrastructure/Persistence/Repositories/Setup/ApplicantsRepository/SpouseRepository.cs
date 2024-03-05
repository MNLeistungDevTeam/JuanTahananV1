using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Services;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class SpouseRepository : ISpouseRepository
    {
        private readonly MNLTemplateDBContext _context;
        private readonly EfCoreHelper<Spouse> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public SpouseRepository(MNLTemplateDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Spouse>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<Spouse?> GetByIdAsync(int id) =>
    await _context.Spouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Spouse?> GetByApplicationInfoIdAsync(int id) =>
     await _context.Spouses.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);

        public async Task<Spouse> SaveAsync(SpouseModel model)
        {
            var _spouce = _mapper.Map<Spouse>(model);

            if (model.Id == 0)

                _spouce = await CreateAsync(_spouce);
            else

                _spouce = await UpdateAsync(_spouce);

            return _spouce;
        }

        public async Task<Spouse> CreateAsync(Spouse model)
        {
            model.DateCreated = DateTime.Now;
            model.CreatedById = _currentUserService.GetCurrentUserId();

            var _spouse = _mapper.Map<Spouse>(model);

            _spouse = await _contextHelper.CreateAsync(_spouse, "DateModified", "ModifiedById");
            return _spouse;
        }

        public async Task<Spouse> UpdateAsync(Spouse model)
        {
            model.DateModified = DateTime.Now;
            model.ModifiedById = _currentUserService.GetCurrentUserId();

            var _spouse = _mapper.Map<Spouse>(model);

            _spouse = await _contextHelper.UpdateAsync(_spouse, "DateCreated", "CreatedById");

            return _spouse;
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
            var entities = await _context.Spouses.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }
    }
}