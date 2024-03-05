using AutoMapper;
using DevExpress.XtraReports.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Interfaces.Setup.RoleRepository;
using Template.Application.Services;
using Template.Domain.Dto.ModuleDto;
using Template.Domain.Dto.RoleDto;
using Template.Domain.Entities;

namespace Template.Infrastructure.Persistence.Repositories.Setup.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly MNLTemplateDBContext _context;
        private readonly EfCoreHelper<Role> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;
        private readonly IRoleAccessRepository _roleAccessRepo;
        public RoleRepository(MNLTemplateDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db, IRoleAccessRepository roleAccessRepo)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Role>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
            _roleAccessRepo = roleAccessRepo;
        }
        public async Task<List<RoleModel>> SpGetAllRoles() =>
        (await _db.LoadDataAsync<RoleModel, dynamic>("spRole_GetAllRoles", new { })).ToList();
        public async Task<Role?> GetByIdAsync(int id) =>
        await _context.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Role>?> GetAllAsync() =>
        await _context.Roles.AsNoTracking().ToListAsync();
        public async Task<Role> SaveAsync(RoleModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<RoleModel>(await CreateAsync(model));
            else
                model = _mapper.Map<RoleModel>(await UpdateAsync(model));
            return _mapper.Map<Role>(model);
        }
        public async Task<Role> CreateAsync(RoleModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            var mapped = _mapper.Map<Role>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified");
            return mapped;
        }
        public async Task<Role> UpdateAsync(RoleModel model)
        {
            model.DateModified = DateTime.UtcNow;
            var mapped = _mapper.Map<Role>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated");
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
            var entities = await _context.Roles.Where(m => ids.Contains(m.Id)).ToListAsync();
            var roleaccess_entities = await _context.RoleAccesses.Where(x => entities.Select(o => o.Id).Contains(x.Id)).ToListAsync();
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
}
