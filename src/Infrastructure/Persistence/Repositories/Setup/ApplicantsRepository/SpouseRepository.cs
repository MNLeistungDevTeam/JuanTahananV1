using AutoMapper;
using DMS.Application.Interfaces.Setup.ApplicantsRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.ApplicantsDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ApplicantsRepository;

public class SpouseRepository : ISpouseRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<Spouse> _contextHelper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ISQLDatabaseService _db;

    public SpouseRepository(
        DMSDBContext context,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ISQLDatabaseService db)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<Spouse>(context);
        _currentUserService = currentUserService;
        _mapper = mapper;
        _db = db;
    }

 
    public async Task<Spouse?> GetByIdAsync(int id) =>
            await _context.Spouses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Spouse>?> GetAllAsync() =>
    await _context.Spouses.AsNoTracking().ToListAsync();

    public async Task<Spouse?> GetByApplicationInfoIdAsync(int id) =>
 await _context.Spouses.AsNoTracking().FirstOrDefaultAsync(x => x.ApplicantsPersonalInformationId == id);

    public async Task<SpouseModel?> GetByApplicantIdAsync(int applicantId) =>
        await _db.LoadSingleAsync<SpouseModel, dynamic>("spSpouse_GetByApplicantId", new { applicantId });

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

        var result = await _contextHelper.CreateAsync(model, "DateModified", "ModifiedById");
        return result;
    }

    public async Task<Spouse> UpdateAsync(Spouse model)
    {
        model.DateModified = DateTime.Now;
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
        var entities = await _context.Spouses.Where(m => ids.Contains(m.Id)).ToListAsync();
        foreach (var entity in entities)
        {
            await DeleteAsync(entity.Id);
        }
    }
}