using AutoMapper;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Domain.Dto.RoleDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.RoleRepository
{
    public class RoleAccessRepository : IRoleAccessRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<RoleAccess> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public RoleAccessRepository(DMSDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<RoleAccess>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<RoleAccess?> GetByIdAsync(int id) =>
        await _context.RoleAccesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<RoleAccess?> GetRoleAccessAsync(int roleId, int ModuleId) =>
            await _context.RoleAccesses.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == roleId && x.ModuleId == ModuleId);

        public async Task<List<RoleAccess>?> GetAllAsync() =>
        await _context.RoleAccesses.AsNoTracking().ToListAsync();

        public async Task<RoleAccess> SaveAsync(RoleAccessModel model)
        {
            var _roleAccess = _mapper.Map<RoleAccess>(model);

            if (model.Id == 0)
                _roleAccess = await CreateAsync(_roleAccess);
            else
                _roleAccess = await UpdateAsync(_roleAccess);

            return _roleAccess;
        }

        public async Task<RoleAccess> CreateAsync(RoleAccess roleAccess)
        {
            roleAccess.DateCreated = DateTime.Now;
            roleAccess.CreatedById = _currentUserService.GetCurrentUserId();

            roleAccess = await _contextHelper.CreateAsync(roleAccess, "DateModified", "ModifiedById");
            return roleAccess;
        }

        public async Task<RoleAccess> UpdateAsync(RoleAccess roleAccess)
        {
            roleAccess.DateModified = DateTime.Now;
            roleAccess.ModifiedById = _currentUserService.GetCurrentUserId();

            roleAccess = await _contextHelper.UpdateAsync(roleAccess, "DateCreated", "CreatedById");
            return roleAccess;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if (entity != null)
                await _contextHelper.DeleteAsync(entity);
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.RoleAccesses.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }
    }
}