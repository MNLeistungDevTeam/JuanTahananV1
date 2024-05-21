using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyManagementRepo;

public class PropertyUnitProjectRepository : IPropertyUnitProjectRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyUnitProject> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public PropertyUnitProjectRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyUnitProject>(context);
        _db = db;
        _mapper = mapper;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyUnitProject?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyUnitProject>> GetAll() =>
        await _contextHelper.GetAllAsync();

    public async Task<IEnumerable<PropertyUnitProjectModel?>> GetPropertyUnitByProjectAsync(int projectId) =>
            await _db.LoadDataAsync<PropertyUnitProjectModel, dynamic>("spPropertyUnit_GetByProjectId", new { projectId });

    #endregion Getters

    #region Operation

    public async Task<PropertyUnitProject> SaveAsync(PropertyUnitProjectModel model, int userId)
    {
        var _model = _mapper.Map<PropertyUnitProject>(model);

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

    public async Task<PropertyUnitProject> CreateAsync(PropertyUnitProject model, int userId)
    {
        model.CreatedById = userId;
        model.DateCreated = DateTime.UtcNow;
        var result = await _contextHelper.CreateAsync(model, "ModifiedById", "DateModified");

        return result;
    }

    public async Task<PropertyUnitProject> UpdateAsync(PropertyUnitProject model, int userId)
    {
        model.ModifiedById = userId;
        model.DateModified = DateTime.UtcNow;
        var result = await _contextHelper.UpdateAsync(model, "CreatedById", "DateCreated");

        return result;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = _context.PropertyUnitProjects.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}