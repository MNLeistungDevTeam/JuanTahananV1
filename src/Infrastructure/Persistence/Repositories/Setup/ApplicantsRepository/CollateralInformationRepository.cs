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
    public class CollateralInformationRepository : ICollateralInformationRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<CollateralInformation> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public CollateralInformationRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<CollateralInformation>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<CollateralInformation?> GetByIdAsync(int id) =>
             await _context.CollateralInformations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        public async Task<CollateralInformation?> GetByApplicationInfoIdAsync(int id) =>
             await _context.CollateralInformations.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);
        public async Task<CollateralInformation> SaveAsync(CollateralInformationModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<CollateralInformationModel>(await CreateAsync(model));
            else
                model = _mapper.Map<CollateralInformationModel>(await UpdateAsync(model));
            return _mapper.Map<CollateralInformation>(model);
        }
        public async Task<CollateralInformation> CreateAsync(CollateralInformationModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<CollateralInformation>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }
        public async Task<CollateralInformation> UpdateAsync(CollateralInformationModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<CollateralInformation>(model);
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
            var entities = await _context.CollateralInformations.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
