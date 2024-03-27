using AutoMapper;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Interfaces.Setup.UserRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.RoleRepository;

public class RoleRepository : IRoleRepository
{
    private readonly DMSDBContext _context;
    private readonly EfCoreHelper<Role> _contextHelper;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ISQLDatabaseService _db;
    private readonly IRoleAccessRepository _roleAccessRepo;
    private readonly IUserRoleRepository _userRoleRepo;

    public RoleRepository(DMSDBContext context,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ISQLDatabaseService db,
        IRoleAccessRepository roleAccessRepo,
        IUserRoleRepository userRoleRepo)
    {
        _context = context;
        _contextHelper = new EfCoreHelper<Role>(context);
        _currentUserService = currentUserService;
        _mapper = mapper;
        _db = db;
        _roleAccessRepo = roleAccessRepo;
        _userRoleRepo = userRoleRepo;
    }

    public async Task<List<RoleModel>> GetAllRolesAsync() =>
        (await _db.LoadDataAsync<RoleModel, dynamic>("spRole_GetAllRoles", new { })).ToList();

    public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Role?> GetByCodeAsync(string code) =>
        await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Name == code);

    public async Task<List<Role>?> GetAllAsync() =>
        await _context.Roles.AsNoTracking().ToListAsync();

    public async Task<RoleModel?> GetByCurrentUser()
    {
        try
        {
            var roles = await GetAllRolesAsync();
            if (roles == null) { return null; }
            if (!roles.Any()) { return null; }

            int currentUserId = _currentUserService.GetCurrentUserId();
            var userRole = await _userRoleRepo.GetUserRoleAsync(currentUserId);
            if (userRole == null) { return null; }

            var role = roles.FirstOrDefault(m => m.Id == userRole.RoleId);

            return role;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Role> SaveAsync(RoleModel rModel, List<RoleAccessModel> raModel)
    {
        try
        {
            var _rModel = _mapper.Map<Role>(rModel);
            var _roleAccessList = new List<RoleAccess>();

            if (_rModel.Id == 0)
            {
                _rModel = await CreateAsync(_rModel);
            }
            else
            {
                _rModel = await UpdateAsync(_rModel);
            }

            foreach (var roleAccess in raModel)
            {
                var _roleAccess = _mapper.Map<RoleAccess>(roleAccess);

                _roleAccess.RoleId = _rModel.Id;

                _roleAccess = await _roleAccessRepo.SaveAsync(_roleAccess);
                _roleAccessList.Add(_roleAccess);
            }

            var roleAccessIds = _roleAccessList.Where(m => m.Id != 0).Select(m => m.Id).ToList();
            var roleAccessToDelete = await _context.RoleAccesses
                .Where(m => m.RoleId == _rModel.Id && !roleAccessIds.Contains(m.Id))
                .ToListAsync();

            await _roleAccessRepo.BatchDeleteAsync(roleAccessToDelete);

            return _rModel;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Role> CreateAsync(Role model)
    {
        model.DateCreated = DateTime.UtcNow;
        var result = await _contextHelper.CreateAsync(model, "DateModified");
        return result;
    }

    public async Task<Role> UpdateAsync(Role model)
    {
        model.DateModified = DateTime.UtcNow;
        var result = await _contextHelper.UpdateAsync(model, "DateCreated");
        return result;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _contextHelper.GetByIdAsync(id);
        if (entity != null)
            await _contextHelper.DeleteAsync(entity);
    }

    public async Task BatchDeleteAsync(int[] ids)
    {
        var entities = await _context.Roles
            .Where(m => ids.Contains(m.Id))
            .ToListAsync();

        var roleaccess_entities = await _context.RoleAccesses
            .Where(x => entities.Select(o => o.Id).Contains(x.Id))
            .ToListAsync();

        foreach (var entity in entities)
        {
            if (!entity.IsLocked)
            {
                await DeleteAsync(entity.Id);
                foreach (var items in roleaccess_entities)
                {
                    await _roleAccessRepo.DeleteAsync(items.Id);
                }
            }
        }
    }
}