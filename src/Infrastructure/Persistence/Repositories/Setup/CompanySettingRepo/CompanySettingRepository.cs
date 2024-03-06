using DMS.Application.Interfaces.Setup.CompanySettingRepo;
using DMS.Application.Services;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.CompanySettingRepo
{


    public class CompanySettingRepository : ICompanySettingRepository
    {
        private readonly ISQLDatabaseService _db;
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<CompanySetting> _contextHelper;

        public CompanySettingRepository(ISQLDatabaseService db, DMSDBContext context)
        {
            _db = db;
            _context = context;
            _contextHelper = new(context);
        }

        #region Read

        public async Task<CompanySetting?> GetByCompanyIdAsync(int companyId) =>
            await _context.CompanySettings.FirstOrDefaultAsync(m => m.CompanyId == companyId);

        public async Task<CompanySetting?> GetByIdAsync(int id)
        {
            var result = await _contextHelper.GetByIdAsync(id);
            return result;
        }

        public async Task<List<CompanySetting>> GetAllAsync()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        #endregion Read

        #region Operation

        public async Task SaveAsync(CompanySetting model, int userId)
        {
            if (model.Id == 0)
                await CreateAsync(model, userId);
            else
                await UpdateAsync(model, userId);
        }

        public async Task<CompanySetting> CreateAsync(CompanySetting module, int userId)
        {
            module.CreatedById = userId;
            module.DateCreated = DateTime.Now;

            string[] exclusions = { "ModifiedById", "DateModified" };
            module = await _contextHelper.CreateAsync(module, exclusions);
            return module;
        }

        public async Task<CompanySetting> UpdateAsync(CompanySetting module, int userId)
        {
            module.ModifiedById = userId;
            module.DateModified = DateTime.Now;

            string[] exclusions = { "CreatedById", "DateCreated" };
            module = await _contextHelper.UpdateAsync(module, exclusions);
            return module;
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var module = _context.CompanySettings.FirstOrDefault(x => x.Id == id);

                if (module is null)
                    return;

                await _contextHelper.DeleteAsync(module);
            }
            catch (Exception) { throw; }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            // Converted to EfCoreHelper method
            var modules = await _context.CompanySettings.Where(x => ids.Contains(x.Id)).ToArrayAsync();

            if (modules is null)
                return;

            await _contextHelper.BatchDeleteAsync(modules);
        }

        #endregion Operation
    }
}
