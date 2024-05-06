using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyManagementRepo;

public class PropertyLocationRepository : IPropertyLocationRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyLocation> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public PropertyLocationRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyLocation>(context);
        _db = db;
        _mapper = mapper;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyLocation?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyLocation>> GetAll() =>
        await _contextHelper.GetAllAsync();

    #endregion Getters

    #region Operation

    public async Task<PropertyLocation> SaveAsync(PropertyLocationModel model, int userId)
    {
        var _model = _mapper.Map<PropertyLocation>(model);

        if (_model.Id == 0)
        {
            await CreateAsync(_model, userId);
        }
        else
        {
            await UpdateAsync(_model, userId);
        }

        return _model;
    }

    public async Task<PropertyLocation> CreateAsync(PropertyLocation model, int userId)
    {
        model.CreatedById = userId;
        model.DateCreated = DateTime.UtcNow;
        var result = await _contextHelper.CreateAsync(model, "ModifiedById", "DateModified");

        return result;
    }

    public async Task<PropertyLocation> UpdateAsync(PropertyLocation model, int userId)
    {
        model.ModifiedById = userId;
        model.DateModified = DateTime.UtcNow;
        var result = await _contextHelper.UpdateAsync(model, "CreatedById", "DateCreated");

        return result;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = _context.PropertyLocations.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}