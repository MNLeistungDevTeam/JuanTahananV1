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
            if (model.Id == 0)
                model = _mapper.Map<Form2PageModel>(await CreateAsync(model));
            else
                model = _mapper.Map<Form2PageModel>(await UpdateAsync(model));
            return _mapper.Map<Form2Page>(model);
        }
        public async Task<Form2Page> CreateAsync(Form2PageModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<Form2Page>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }
        public async Task<Form2Page> UpdateAsync(Form2PageModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<Form2Page>(model);
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
            var entities = await _context.Form2Pages.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
