using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class LoanParticularsInformationRepository : ILoanParticularsInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<LoanParticularsInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public LoanParticularsInformationRepository(
            DMSDBContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<LoanParticularsInformation>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<LoanParticularsInformation?> GetByIdAsync(int id) =>
            await _context.LoanParticularsInformations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<LoanParticularsInformation?> GetByApplicationIdAsync(int id) =>
            await _context.LoanParticularsInformations.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);

        public async Task<LoanParticularsInformation> SaveAsync(LoanParticularsInformationModel model)
        {
            var _model = _mapper.Map<LoanParticularsInformation>(model);

            if (_model.Id == 0)
                _model = await CreateAsync(_model);
            else
                _model = await UpdateAsync(_model);

            return _model;
        }

        public async Task<LoanParticularsInformation> CreateAsync(LoanParticularsInformation model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();

            model = await _contextHelper.CreateAsync(model, "DateModified", "ModifiedById");
            return model;
        }

        public async Task<LoanParticularsInformation> UpdateAsync(LoanParticularsInformation model)
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
            var entities = await _context.LoanParticularsInformations.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }
    }
}