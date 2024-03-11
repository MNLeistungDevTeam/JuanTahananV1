using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class BarrowersInformationRepository : IBarrowersInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<BarrowersInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public BarrowersInformationRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<BarrowersInformation>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<BarrowersInformation?> GetByIdAsync(int id) =>
            await _context.BarrowersInformations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<BarrowersInformation?> GetByApplicationInfoIdAsync(int id) =>
            await _context.BarrowersInformations.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);

        public async Task<BarrowersInformationModel?> GetByApplicantIdAsync(int applicantId) =>
        await _db.LoadSingleAsync<BarrowersInformationModel, dynamic>("spBarrowersInformationModel_GetByApplicantId", new { applicantId });

        public async Task<BarrowersInformation> SaveAsync(BarrowersInformationModel model)
        {

            var _barrowerInfo = _mapper.Map<BarrowersInformation>(model);

            if (model.Id == 0)
                _barrowerInfo = await CreateAsync(_barrowerInfo);
            else
                _barrowerInfo = await UpdateAsync(_barrowerInfo);
            return _barrowerInfo;
        }

        public async Task<BarrowersInformation> CreateAsync(BarrowersInformation barrower)
        {
            barrower.DateCreated = DateTime.Now;

            barrower.CreatedById = _currentUserService.GetCurrentUserId();

            barrower = await _contextHelper.CreateAsync(barrower, "DateModified", "ModifiedById");
            return barrower;
        }

        public async Task<BarrowersInformation> UpdateAsync(BarrowersInformation barrower)
        {
            barrower.DateModified = DateTime.UtcNow;
            barrower.ModifiedById = _currentUserService.GetCurrentUserId();

            barrower = await _contextHelper.UpdateAsync(barrower, "DateCreated", "CreatedById");
            return barrower;
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
            var entities = await _context.BarrowersInformations.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }
    }
}