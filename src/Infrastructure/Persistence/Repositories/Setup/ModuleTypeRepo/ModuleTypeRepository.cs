using AutoMapper;
using DMS.Application.Interfaces.Setup.ModuleTypeRepo;
using DMS.Domain.Dto.ModuleTypeDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.ModuleTypeRepo
{
    public class ModuleTypeRepository : IModuleTypeRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<ModuleType> _contextHelper;
        private readonly IMapper _mapper;

        public ModuleTypeRepository(DMSDBContext context, IMapper mapper)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<ModuleType>(context);
            _mapper = mapper;
        }

        #region Get Methods

        public async Task<ModuleType?> GetByIdAsync(int id) =>
            await _contextHelper.GetByIdAsync(id);

        public async Task<List<ModuleType>> GetAllAsync() =>
            await _contextHelper.GetAllAsync();

        #endregion Get Methods

        #region Action Methods

        public async Task<ModuleType> SaveAsync(ModuleTypeModel moduleType, int userId)
        {
            var _moduleType = _mapper.Map<ModuleType>(moduleType);

            if (_moduleType.Id == 0)
            {
                _moduleType = await CreateAsync(_moduleType, userId);
            }
            else
            {
                _moduleType = await UpdateAsync(_moduleType, userId);
            }

            return _moduleType;
        }

        public async Task<ModuleType> CreateAsync(ModuleType moduleType, int userId)
        {
            moduleType.CreatedById = userId;
            moduleType.DateCreated = DateTime.UtcNow;

            var result = await _contextHelper.CreateAsync(moduleType, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<ModuleType> UpdateAsync(ModuleType moduleType, int userId)
        {
            moduleType.ModifiedById = userId;
            moduleType.DateModified = DateTime.UtcNow;

            var result = await _contextHelper.UpdateAsync(moduleType, "ModifiedById", "DateModified");

            return result;
        }

        public async Task BachDeleteAsync(int[] ids)
        {
            var moduleTypes = await _context.ModuleTypes
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();

            if (moduleTypes is not null)
            {
                await _contextHelper.BatchDeleteAsync(moduleTypes);
            }
        }

        #endregion Action Methods
    }
}