﻿using AutoMapper;
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

    public PropertyProjectRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper,
        IPropertyProjectLocationRepository propertyprojectLocationRepo)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyProject>(context);
        _db = db;
        _mapper = mapper;
        _propertyprojectLocationRepo = propertyprojectLocationRepo;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyProject?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyProject>> GetAll() =>
        await _contextHelper.GetAllAsync();

    public async Task<IEnumerable<PropertyProjectModel?>> GetAllAsync() =>
           await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spPropertyProject_GetAll", new { });

    public async Task<IEnumerable<PropertyProjectModel?>> GetByCompanyAsync(int companyId) =>
           await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spProject_GetByCompanyId", new { companyId });


    public async Task<IEnumerable<PropertyProjectModel?>> GetPropertyLocationByProjectAsync(int id) =>
       await _db.LoadDataAsync<PropertyProjectModel, dynamic>("spProject_GetPropertyLocationByProjectId", new { id });




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

    public async Task SaveProjectLocations(PropertyProjectModel project, List<PropertyProjectLocationModel> userProjectList, int userId)
    {
        try
        {
            if (project == null) return;

            var userCounter = 1;

            var _userProjectList = _mapper.Map<List<PropertyProjectLocation>>(userProjectList);

            foreach (var address in _userProjectList)
            {
                if (address.Id == 0)
                    await _propertyprojectLocationRepo.CreateAsync(address, userId);
                else
                    await _propertyprojectLocationRepo.UpdateAsync(address, userId);

                userCounter++;
            }

            // clean up for unused stages
            var userIds = _userProjectList.Where(m => m.Id != 0).Select(m => m.Id).ToList();

            var toDelete = await _context.PropertyProjectLocations
                .Where(m => m.ProjectId == project.Id && !userIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToArrayAsync();

            if (toDelete is not null && toDelete.Any())
                await _propertyprojectLocationRepo.BatchDeleteAsync(toDelete);
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