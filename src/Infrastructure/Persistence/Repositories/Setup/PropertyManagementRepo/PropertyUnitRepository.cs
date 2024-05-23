using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyManagementRepo;

public class PropertyUnitRepository : IPropertyUnitRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyUnit> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public PropertyUnitRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyUnit>(context);
        _db = db;
        _mapper = mapper;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyUnit?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyUnit>> GetAll() =>
        await _contextHelper.GetAllAsync();

    public async Task<IEnumerable<PropertyUnitModel?>> GetUnitByProjectAsync(int? projectId, int? developerId) =>
       await _db.LoadDataAsync<PropertyUnitModel, dynamic>("spPropertyUnit_GetUnitByProjectId", new { projectId, developerId });

    #endregion Getters

    #region Operation

    public async Task<PropertyUnit> SaveAsync(PropertyUnitModel model, int userId)
    {
        var _model = _mapper.Map<PropertyUnit>(model);

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

    public async Task<PropertyUnit> CreateAsync(PropertyUnit model, int userId)
    {
        try
        {
            model.CreatedById = userId;
            model.DateCreated = DateTime.UtcNow;
            var result = await _contextHelper.CreateAsync(model, "ModifiedById", "DateModified");

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PropertyUnit> UpdateAsync(PropertyUnit model, int userId)
    {
        List<string> excludedPropertiesList = new List<string>
            {
                 "CreatedById",
                "DateCreated"
            };

        if (string.IsNullOrWhiteSpace(model.ProfileImage))
        {
            excludedPropertiesList.Add("ProfileImage");
        }

        // Convert list to array
        string[] excludedProperties = excludedPropertiesList.ToArray();

        model.ModifiedById = userId;
        model.DateModified = DateTime.Now;

        var result = await _contextHelper.UpdateAsync(model, excludedProperties);

        return result;

      
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = _context.PropertyUnits.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}