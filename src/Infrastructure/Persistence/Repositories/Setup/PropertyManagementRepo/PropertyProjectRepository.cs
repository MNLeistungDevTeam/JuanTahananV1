using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyProjectRepo;

public class PropertyProjectRepository : IPropertyProjectRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyProject> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public PropertyProjectRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyProject>(context);
        _db = db;
        _mapper = mapper;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyProject?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyProject>> GetAll() =>
        await _contextHelper.GetAllAsync();

    public async Task<IEnumerable<PropertyProjectModel?>> GetAllAsync() =>
           await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spPropertyProject_GetAll", new { });



    public async Task<PropertyProjectModel?> GetByCompanyAsync(int companyId) =>
           await _db.LoadSingleAsync<PropertyProjectModel, dynamic>("spProject_GetByCompanyId", new { companyId });

    #endregion Getters

    #region Operation

    public async Task<PropertyProject> SaveAsync(PropertyProjectModel model, int userId)
    {
        var _model = _mapper.Map<PropertyProject>(model);

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

    public async Task<PropertyProject> CreateAsync(PropertyProject model, int userId)
    {
        model.CreatedById = userId;
        model.DateCreated = DateTime.UtcNow;
        var result = await _contextHelper.CreateAsync(model, "ModifiedById", "DateModified");

        return result;
    }

    public async Task<PropertyProject> UpdateAsync(PropertyProject model, int userId)
    {
        model.ModifiedById = userId;
        model.DateModified = DateTime.UtcNow;
        var result = await _contextHelper.UpdateAsync(model, "CreatedById", "DateCreated");

        return result;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = _context.PropertyProjects.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}