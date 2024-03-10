using DMS.Application.Interfaces.Setup.CountryRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EntityDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.CountryRepo
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ISQLDatabaseService _db;
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Country> _contextHelper;

        public CountryRepository(ISQLDatabaseService db, DMSDBContext context)
        {
            _db = db;
            _context = context;
            _contextHelper = new(context);
        }

        #region Read

        public async Task<CountryModel?> GetByIdAsync(int id) =>
            await _db.LoadSingleAsync<CountryModel, dynamic>("spCountry_Get", new { id });

        public async Task<IEnumerable<CountryModel>> GetAllAsync() =>
            await _db.LoadDataAsync<CountryModel, dynamic>("spCountry_GetAll", new { });

        #endregion Read

        #region Operation

        public async Task SaveAsync(Country model, int userId)
        {
            try
            {
                await ValidateCountry(model);

                // Converted to EfCoreHelper method
                if (model.Id == 0)
                    await CreateAsync(model, userId);
                else
                    await UpdateAsync(model, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Country> CreateAsync(Country module, int userId)
        {
            module.CreatedById = userId;
            module.DateCreated = DateTime.Now;

            string[] exclusions = { "ModifiedById", "DateModified" };
            module = await _contextHelper.CreateAsync(module, exclusions);
            return module;
        }

        public async Task<Country> UpdateAsync(Country module, int userId)
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
                var module = _context.Countries.FirstOrDefault(x => x.Id == id);

                if (module is null)
                    return;

                await _contextHelper.DeleteAsync(module);
            }
            catch (Exception) { throw; }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            // Converted to EfCoreHelper method
            var modules = await _context.Countries.Where(x => ids.Contains(x.Id)).ToArrayAsync();

            if (modules is null)
                return;

            await _contextHelper.BatchDeleteAsync(modules);
        }

        #endregion Operation

        #region Validation

        private async Task ValidateCountry(Country model)
        {
            try
            {
                var modules = await GetAllAsync();

                if (model.Id == 0)
                {
                    if (modules.FirstOrDefault(m => m.Code == model.Code) != null) throw new Exception("Country Code already exists!");
                    if (modules.FirstOrDefault(m => m.Name == model.Name) != null) throw new Exception("Country Name already exists!");
                }
                else
                {
                    if (modules.FirstOrDefault(m => m.Id != model.Id && m.Code == model.Code) != null) throw new Exception("Country Code already exists!");
                    if (modules.FirstOrDefault(m => m.Id != model.Id && m.Name == model.Name) != null) throw new Exception("Country Name already exists!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Validation
    }
}
