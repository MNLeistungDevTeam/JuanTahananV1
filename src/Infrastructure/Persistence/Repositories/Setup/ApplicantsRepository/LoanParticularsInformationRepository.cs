using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository
{
    public class LoanParticularsInformationRepository : ILoanParticularsInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<LoanParticularsInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public LoanParticularsInformationRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
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

        public async Task<LoanParticularsInformationModel?> GetByApplicantIdAsync(int applicantId) =>
            await _db.LoadSingleAsync<LoanParticularsInformationModel, dynamic>("spLoanParticularsInformation_GetByApplicantId", new { applicantId });

        public async Task<LoanParticularsInformation> SaveAsync(LoanParticularsInformationModel model, int userId)
        {
            var _loanParticulars = _mapper.Map<LoanParticularsInformation>(model);
            if (model.Id == 0)
                _loanParticulars = await CreateAsync(_loanParticulars, userId);
            else
                _loanParticulars = await UpdateAsync(_loanParticulars, userId);
            return _loanParticulars;
        }

        public async Task<LoanParticularsInformation> CreateAsync(LoanParticularsInformation loanParticulars, int userId)
        {
            loanParticulars.DateCreated = DateTime.Now;
            loanParticulars.CreatedById = userId;

            loanParticulars = await _contextHelper.CreateAsync(loanParticulars, "DateModified", "ModifiedById");
            return loanParticulars;
        }

        public async Task<LoanParticularsInformation> UpdateAsync(LoanParticularsInformation loanParticulars, int userId)
        {
            loanParticulars.DateModified = DateTime.Now;
            loanParticulars.ModifiedById = userId;

            loanParticulars = await _contextHelper.UpdateAsync(loanParticulars, "DateCreated", "CreatedById");
            return loanParticulars;
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