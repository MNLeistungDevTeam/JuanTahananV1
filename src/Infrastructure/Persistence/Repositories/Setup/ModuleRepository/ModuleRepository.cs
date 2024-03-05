using AutoMapper;
using DevExpress.XtraReports.Design;
using DMS.Domain.Dto.ModuleDto;
using Microsoft.EntityFrameworkCore;
using DMS.Application.Interfaces.Setup.ModuleRepository;
using DMS.Application.Services;
using DMS.Domain.Entities;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModuleRepository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly MNLTemplateDBContext _context;
        private readonly EfCoreHelper<Module> _contextHelper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public ModuleRepository(MNLTemplateDBContext context, ICurrentUserService currentUserService, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<Module>(context);
            _currentUserService = currentUserService;
            _mapper = mapper;
            _db = db;
        }
        public async Task<Module?> GetByIdAsync(int id) =>
            await _context.Modules.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        public async Task<List<ModuleModel>> Module_GetAllModuleList() =>
        (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetAllModules", new { })).ToList();
        public async Task<List<ModuleModel>> SpGetAllUserModules() =>
        (await _db.LoadDataAsync<ModuleModel, dynamic>("spModule_GetAllUserModules", new { userId = _currentUserService.GetCurrentUserId()})).ToList();
        public async Task<Module> SaveAsync(ModuleModel model)
        {
            if (model.Id == 0)
                model = _mapper.Map<ModuleModel>(await CreateAsync(model));
            else
                model = _mapper.Map<ModuleModel>(await UpdateAsync(model));
            return _mapper.Map<Module>(model);
        }
        public async Task<Module> CreateAsync(ModuleModel model)
        {
            model.DateCreated = DateTime.UtcNow;
            model.CreatedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<Module>(model);
            mapped = await _contextHelper.CreateAsync(mapped, "DateModified", "ModifiedById");
            return mapped;
        }
        public async Task<Module> UpdateAsync(ModuleModel model)
        {
            model.DateModified = DateTime.UtcNow;
            model.ModifiedById = _currentUserService.GetCurrentUserId();
            var mapped = _mapper.Map<Module>(model);
            mapped = await _contextHelper.UpdateAsync(mapped, "DateCreated", "CreatedById");
            return mapped;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _contextHelper.GetByIdAsync(id);
            if(entity != null)
            {
                entity.DateDeleted = DateTime.Now;
                entity.DeletedById = _currentUserService.GetCurrentUserId();
                if (entity is not null)
                    await _contextHelper.UpdateAsync(entity);
            }
        }
        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = await _context.Modules.Where(m => ids.Contains(m.Id)).ToListAsync();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity.Id);
            }

        }
    }
}
