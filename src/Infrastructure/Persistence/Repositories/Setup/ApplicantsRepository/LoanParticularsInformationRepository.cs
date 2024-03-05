using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.ApplicantsRepository;
using Template.Application.Services;
using Template.Domain.Dto.ApplicantsDto;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class LoanParticularsInformationRepository : ILoanParticularsInformationRepository
    {
        private readonly MNLTemplateDBContext _context;
        private readonly EfCoreHelper<LoanParticularsInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public LoanParticularsInformationRepository(MNLTemplateDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
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
            if (model.Id == 0)
                model = _mapper.Map<LoanParticularsInformationModel>(await CreateAsync(model));
            else
                model = _mapper.Map<LoanParticularsInformationModel>(await UpdateAsync(model));
            return _mapper.Map<LoanParticularsInformation>(model);
        }
        public async Task<LoanParticularsInformation> CreateAsync(LoanParticularsInformationModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<LoanParticularsInformation>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }
        public async Task<LoanParticularsInformation> UpdateAsync(LoanParticularsInformationModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<LoanParticularsInformation>(model);
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
            var entities = await _context.LoanParticularsInformations.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
