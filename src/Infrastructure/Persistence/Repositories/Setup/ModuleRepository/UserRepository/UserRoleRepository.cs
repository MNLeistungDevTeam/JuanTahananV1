using AutoMapper;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.UserDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.UserRepository;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<UserRole> _contextHelper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ISQLDatabaseService _db;

    public UserRoleRepository(
        DMSDBContext context,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ISQLDatabaseService db)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<UserRole>(context);
        _currentUserService = currentUserService;
        _mapper = mapper;
        _db = db;
    }

    public async Task<List<UserRoleModel>> SpGetAllRoles() =>
    (await _db.LoadDataAsync<UserRoleModel, dynamic>("spUserRole_GetAllRoles", new { })).ToList();

    public async Task<UserRole?> GetByIdAsync(int id) =>
    await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<UserRole?> GetUserRoleAsync(int UserId) =>
        await _context.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == UserId);

    public async Task<List<UserRole>?> GetAllAsync() =>
    await _context.UserRoles.AsNoTracking().ToListAsync();

    public async Task<UserRole> SaveAsync(UserRoleModel model)
    {
        try
        {
            var _model = _mapper.Map<UserRole>(model);

            int _modelId = await _context.UserRoles
                .AsNoTracking()
                .Where(m => m.UserId == _model.UserId)
                .Select(m => (int?) m.Id)
                .FirstOrDefaultAsync() ?? 0;

            if (_modelId == 0)
                _model = await CreateAsync(_model);
            else
            {
                _model.Id = _modelId;
                _model = await UpdateAsync(_model);
            }

            return _model;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UserRole> CreateAsync(UserRole model)
    {
        return await _contextHelper.CreateAsync(model);
    }

    public async Task<UserRole> UpdateAsync(UserRole model)
    {
        return await _contextHelper.UpdateAsync(model);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _contextHelper.GetByIdAsync(id);
        if (entity != null)
            await _contextHelper.DeleteAsync(entity);
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = await _context.UserRoles.
            Where(m => ids.Contains(m.Id))
            .ToListAsync();

        if (entities is not null)
            await _contextHelper.BatchDeleteAsync(entities);
    }

    public async Task SaveBenificiaryAsync(int userId)
    {
        UserRoleModel uRole = new()
        {
            UserId = userId,
            RoleId = 4//beneficiary need have type enums setup
        };

        await SaveAsync(uRole);
    }
}