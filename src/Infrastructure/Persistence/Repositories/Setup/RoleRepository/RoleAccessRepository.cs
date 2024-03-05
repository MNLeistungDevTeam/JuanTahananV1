using AutoMapper;
using DMS.Domain.Dto.RoleDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Application.Interfaces.Setup.RoleRepository;
using DMS.Application.Services;
using DMS.Domain.Entities;

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
            if (model.Id == 0)
                model = _mapper.Map<RoleAccessModel>(await CreateAsync(model));
            else
                model = _mapper.Map<RoleAccessModel>(await UpdateAsync(model));
            return _mapper.Map<RoleAccess>(model);
        }
        public async Task<RoleAccess> CreateAsync(RoleAccessModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<RoleAccess>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified","ModifiedById");
            return mapped;
        }
        public async Task<RoleAccess> UpdateAsync(RoleAccessModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<RoleAccess>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated","CreatedById");
            return mapped;
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
