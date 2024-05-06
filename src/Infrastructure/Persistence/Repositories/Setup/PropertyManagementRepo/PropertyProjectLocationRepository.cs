﻿using AutoMapper;
using DMS.Application.Interfaces.Setup.PropertyManagementRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.PropertyManagementDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.PropertyManagementRepo;

public class PropertyProjectLocationRepository : IPropertyProjectLocationRepository
{
    #region Fields

    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<PropertyProjectLocation> _contextHelper;
    private readonly ISQLDatabaseService _db;
    private readonly IMapper _mapper;

    public PropertyProjectLocationRepository(
        DMSDBContext context,
        ISQLDatabaseService db,
        IMapper mapper)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<PropertyProjectLocation>(context);
        _db = db;
        _mapper = mapper;
    }

    #endregion Fields

    #region Getters

    public async Task<PropertyProjectLocation?> GetById(int id) =>
        await _contextHelper.GetByIdAsync(id);

    public async Task<List<PropertyProjectLocation>> GetAll() =>
        await _contextHelper.GetAllAsync();

    #endregion Getters

    #region Operation

    public async Task<PropertyProjectLocation> SaveAsync(PropertyProjectLocationModel model, int userId)
    {
        var _model = _mapper.Map<PropertyProjectLocation>(model);

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

    public async Task<PropertyProjectLocation> CreateAsync(PropertyProjectLocation model, int userId)
    {
        model.CreatedById = userId;
        model.DateCreated = DateTime.UtcNow;
        var result = await _contextHelper.CreateAsync(model, "ModifiedById", "DateModified");

        return result;
    }

    public async Task<PropertyProjectLocation> UpdateAsync(PropertyProjectLocation model, int userId)
    {
        model.ModifiedById = userId;
        model.DateModified = DateTime.UtcNow;
        var result = await _contextHelper.UpdateAsync(model, "CreatedById", "DateCreated");

        return result;
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = _context.PropertyProjectLocations.Where(m => ids.Contains(m.Id));

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    #endregion Operation
}