using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyProjectRepo;

public class PropertyProjectRepository : IPropertyProjectRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyProject> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;
    private readonly IPropertyProjectLocationRepository _propertyprojectLocationRepo;
    private readonly IPropertyUnitProjectRepository _propertyunitProjectRepository;

    public PropertyProjectRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper,
        IPropertyProjectLocationRepository propertyprojectLocationRepo,
        IPropertyUnitProjectRepository propertyunitProjectRepository)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyProject>(context);
        _db = db;
        _mapper = mapper;
        _propertyprojectLocationRepo = propertyprojectLocationRepo;
        _propertyunitProjectRepository = propertyunitProjectRepository;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyProject?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyProject>> GetAll() =>
        await _contextHelper.GetAllAsync();

    public async Task<IEnumerable<PropertyProjectModel?>> GetAllAsync() =>
           await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spPropertyProject_GetAll", new { });

    public async Task<IEnumerable<PropertyProjectModel?>> GetByCompanyAsync(int companyId, int? locationId) =>
           await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spProject_GetByCompanyId", new { companyId, locationId });

    public async Task<IEnumerable<PropertyProjectModel?>> GetPropertyLocationByProjectAsync(int id) =>
       await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spProject_GetPropertyLocationByProjectId", new { id });

    public async Task<IEnumerable<PropertyProjectModel?>> GetPropertyUnitByProjectAsync(int id) =>
        await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spProject_GetPropertyUnitByProjectId", new { id });

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

    public async Task SaveProjectLocations(PropertyProjectModel project, List<PropertyProjectLocationModel> locations, int userId)
    {
        //try
        //{
        //    if (project == null) return;

        //    var userCounter = 1;
        //    var _locations = _mapper.Map<List<PropertyProjectLocation>>(locations);

        //    foreach (var location in _locations)
        //    {

        //        if (location.Id == 0)
        //        {
        //            await _propertyprojectLocationRepo.CreateAsync(location, userId);
        //        }
        //        else
        //        {
        //            await _propertyprojectLocationRepo.CreateAsync(location, userId);
        //        }

        //        userCounter++;
        //    }

        //    // clean up for unused project
        //    var userIds = _locations.Where(m => m.Id != 0).Select(m => m.Id).ToList();

        //    var toDelete = await _context.PropertyProjectLocations
        //       .Where(m => m.ProjectId == project.Id && !userIds.Contains(m.Id))
        //       .Select(m => m.Id)
        //       .ToArrayAsync();

        //    if (toDelete is not null && userIds.Any())
        //    {
        //        await _propertyprojectLocationRepo.BatchDeleteAsync(toDelete);
        //    }

        //}
        //catch (Exception)
        //{
        //    throw;
        //}

        try
        {
            var entityToCompare = new List<PropertyProjectLocation>();

            foreach (var location in locations)
            {
                var existingLocation = await _context.PropertyProjectLocations
                    .FirstOrDefaultAsync(x => x.ProjectId == project.Id && x.LocationId == location.LocationId);

                if (existingLocation == null)
                {
                    // Create new location
                    var _location = new PropertyProjectLocation
                    {
                        ProjectId = project.Id,
                        LocationId = location.LocationId,
                        CreatedById = userId,
                        DateCreated = DateTime.UtcNow
                    };

                    var _model = await _propertyprojectLocationRepo.CreateAsync(_location, userId);
                    entityToCompare.Add(_model);
                }
                else
                {
                    entityToCompare.Add(existingLocation);
                }
            }

            // clean up for unused project
            var userIds = entityToCompare.Where(m => m.Id != 0).Select(m => m.Id).ToList();

            var toDelete = await _context.PropertyProjectLocations
                  .Where(m => m.ProjectId == project.Id && !userIds.Contains(m.Id))
                  .Select(m => m.Id)
                  .ToArrayAsync();

            if (toDelete.Any())
            {
                await _propertyprojectLocationRepo.BatchDeleteAsync(toDelete);
            }


        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SaveProjectUnits(PropertyProjectModel project, List<PropertyUnitProjectModel> units, int userId)
    {
        //try
        //{
        //    if (project == null) return;

        //    var userCounter = 1;

        //    var _userUnitList = _mapper.Map<List<PropertyUnitProject>>(userUnitList);

        //    foreach (var unit in _userUnitList)
        //    {
        //        if (unit.Id == 0)
        //            await _propertyunitProjectRepository.CreateAsync(unit, userId);
        //        else
        //            await _propertyunitProjectRepository.UpdateAsync(unit, userId);

        //        userCounter++;
        //    }

        //    // clean up for unused stages
        //    var userIds = _userUnitList.Where(m => m.Id != 0).Select(m => m.Id).ToList();

        //    var toDelete = await _context.PropertyUnitProjects
        //        .Where(m => m.ProjectId == project.Id && !userIds.Contains(m.Id))
        //        .Select(m => m.Id)
        //        .ToArrayAsync();

        //    if (toDelete is not null && toDelete.Any())
        //        await _propertyunitProjectRepository.BatchDeleteAsync(toDelete);
        //}
        //catch (Exception)
        //{
        //    throw;
        //}

        try
        {
            var entityToCompare = new List<PropertyUnitProject>();

            foreach (var unit in units)
            {
                var existingLocation = await _context.PropertyUnitProjects
                    .FirstOrDefaultAsync(x => x.ProjectId == project.Id && x.UnitId == unit.UnitId);

                if (existingLocation == null)
                {
                    // Create new location
                    var newLocation = new PropertyUnitProject
                    {
                        ProjectId = project.Id,
                        UnitId = unit.UnitId,
                        CreatedById = userId,
                        DateCreated = DateTime.UtcNow
                    };

                    var _model = await _propertyunitProjectRepository.CreateAsync(newLocation, userId);
                    entityToCompare.Add(_model);
                }
                else
                {
                    entityToCompare.Add(existingLocation);
                }
            }

            // clean up for unused project
            var userIds = entityToCompare.Where(m => m.Id != 0).Select(m => m.Id).ToList();

            var toDelete = await _context.PropertyUnitProjects
               .Where(m => m.ProjectId == project.Id && !userIds.Contains(m.Id))
               .Select(m => m.Id)
               .ToArrayAsync();


            if (toDelete.Any())
            {
                await _propertyunitProjectRepository.BatchDeleteAsync(toDelete);
            }

        }
        catch (Exception)
        {
            throw;
        }

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
        var entities = _context.PropertyProjects.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}