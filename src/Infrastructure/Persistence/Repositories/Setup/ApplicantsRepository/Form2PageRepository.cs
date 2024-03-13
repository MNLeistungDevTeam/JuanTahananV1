using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class Form2PageRepository : IForm2PageRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Form2Page> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public Form2PageRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Form2Page>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<Form2Page?> GetByIdAsync(int id) =>
            await _context.Form2Pages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Form2Page?> GetByApplicationInfoIdAsync(int id) =>
            await _context.Form2Pages.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);

        public async Task<Form2PageModel?> GetByApplicantIdAsync(int applicantId) =>
      await _db.LoadSingleAsync<Form2PageModel, dynamic>("spForm2Page_GetByApplicantId", new { applicantId });

        public async Task<Form2Page> SaveAsync(Form2PageModel model)
        {
            var _model = _mapper.Map<Form2Page>(model);

            if (_model.Id == 0)
                _model = await CreateAsync(_model);
            else
                _model = await UpdateAsync(_model);

            return _model;
        }

        public async Task<Form2Page> CreateAsync(Form2Page model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();

            var result = await _contextHelper.CreateAsync(model, "DateModified", "ModifiedById");
            return result;
        }

        public async Task<Form2Page> UpdateAsync(Form2Page model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();

            var result = await _contextHelper.UpdateAsync(model, "DateCreated", "CreatedById");
            return result;
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
            var entities = await _context.Form2Pages.Where(m => ids.Contains(m.Id)).ToListAsync();
            
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities); 
            }
        }
    }
}