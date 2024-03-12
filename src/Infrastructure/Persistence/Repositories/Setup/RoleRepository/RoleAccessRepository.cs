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

        public RoleAccessRepository(
            DMSDBContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<RoleAccess>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }

        public async Task<RoleAccess?> GetByIdAsync(int id) =>
        await _context.RoleAccesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<RoleAccessModel>> GetByUserId(int userId)
        {
            var data = await _db.LoadDataAsync<RoleAccessModel, dynamic>("spRoleAccess_GetByUserId", new { userId });
            return data.ToList();
        }

        public async Task<RoleAccessModel> GetByModuleCode(int userId, string moduleCode)
        {
            var data = await _db.LoadSingleAsync<RoleAccessModel, dynamic>("spRoleAccess_GetByModuleCode", new { userId, moduleCode });
            return data ?? new();
        }

        public async Task<RoleAccessModel> GetCurrentUserRoleAccessByModuleAsync(string moduleCode)
        {
            var userId = _currentUserService.GetCurrentUserId();
            return await GetByModuleCode(userId, moduleCode);
        }

        public async Task<List<RoleAccess>?> GetAllAsync() =>
        await _context.RoleAccesses.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<RoleAccessModel>> GetRoleByModuleCodeAsync(int userId, string? moduleCode) =>
          await _db.LoadDataAsync<RoleAccessModel, dynamic>("spRoleAccess_GetRoleByModuleCode", new { userId, moduleCode });

        public async Task<RoleAccess> SaveAsync(RoleAccess model)
        {
            if (model.Id == 0)
                model = await CreateAsync(model);
            else
                model = await UpdateAsync(model);

            return model;
        }

        public async Task<RoleAccess> CreateAsync(RoleAccess roleAccess)
        {
            roleAccess.DateCreated = DateTime.Now;
            roleAccess.CreatedById = _currentUserService.GetCurrentUserId();

            var result = await _contextHelper.CreateAsync(roleAccess, "DateModified", "ModifiedById");
            return result;
        }

        public async Task<RoleAccess> UpdateAsync(RoleAccess roleAccess)
        {
            roleAccess.DateModified = DateTime.Now;
            roleAccess.ModifiedById = _currentUserService.GetCurrentUserId();

            var result = await _contextHelper.UpdateAsync(roleAccess, "DateCreated", "CreatedById");
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
            var entities = await _context.RoleAccesses.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }
        }

        public async Task BatchDeleteAsync(List<RoleAccess> roleAccessList)
        {
            await _contextHelper.BatchDeleteAsync(roleAccessList);
        }
    }
}