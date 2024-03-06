using DMS.Application.Interfaces.Setup.CompanyLogoRepo;
using DMS.Application.Interfaces.Setup.CompanyRepo;
using DMS.Application.Interfaces.Setup.CompanySettingRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Entities;
using DMS.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.CompanyRepo
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ISQLDatabaseService _db;
        private readonly ICompanySettingRepository _companySetting;
        private readonly ICompanyLogoRepository _companyLogo;
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<Company> _contextHelper;

        public CompanyRepository(
            ISQLDatabaseService db,
            DMSDBContext context,
            ICompanySettingRepository companySetting,
            ICompanyLogoRepository companyLogo)
        {
            _db = db;
            _context = context;
            _companyLogo = companyLogo;
            _companySetting = companySetting;
            _contextHelper = new(context);
        }

        #region Read

        public async Task<CompanyModel?> GetByIdAsync(int id) =>
            await _db.LoadSingleAsync<CompanyModel, dynamic>("spCompany_Get", new { id });

        public async Task<Company?> GetByIdAsync2(int companyId) =>
            await _context.Companies.FirstOrDefaultAsync(m => m.Id == companyId);

        public async Task<IEnumerable<CompanyModel>> GetAllAsync() =>
            await _db.LoadDataAsync<CompanyModel, dynamic>("spCompany_GetAll", new { });

        // part of it?
        public async Task<CompanyInfoModel> GetInfoAsync(int companyId) =>
            await _db.LoadSingleAsync<CompanyInfoModel, dynamic>("spCompany_GetInfo", new { companyId });

        public async Task<Company?> GetByCodeAsync(string code) =>
            await _context.Companies.FirstOrDefaultAsync(m => m.Code == code);

        #endregion Read

        #region Validation

        private async Task ValidateCompany(Company cModel)
        {
            try
            {
                var companies = await GetAllAsync();

                if (cModel.Id == 0)
                {
                    if (companies.FirstOrDefault(m => m.Name == cModel.Name) != null)
                        throw new Exception("Company Name already exists!");

                    if (companies.FirstOrDefault(m => m.Code == cModel.Code) != null)
                        throw new Exception("Company Code already exists!");
                }
                else
                {
                    if (companies.FirstOrDefault(m => m.Id != cModel.Id && m.Name == cModel.Name) != null)
                        throw new Exception("Company Name already exists!");

                    if (companies.FirstOrDefault(m => m.Id != cModel.Id && m.Code == cModel.Code) != null)
                        throw new Exception("Company Code already exists!");
                }
            }
            catch (Exception) { throw; }
        }

        #endregion Validation

        #region Operation

        public async Task SaveAsync(Company model, List<CompanyLogo> clModel, List<Address> aModel, int userId, string webHostingEnv, CompanySetting csModel)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await ValidateCompany(model);

                if (model.Id == 0)
                    model = await CreateAsync(model, userId);
                else
                    model = await UpdateAsync(model, userId);

                // Changed
                await _companySetting.SaveAsync(csModel, userId);

                foreach (var item in clModel)
                {
                    item.CompanyId = model.Id;

                    if (item.Id == 0)
                        await _companyLogo.CreateAsync(item, userId);
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.Location))
                            await _companyLogo.UpdateAsync(item, userId, "Location");
                        else
                            await _companyLogo.UpdateAsync(item, userId);
                    }
                }

                // Later to be revised?
                foreach (var item in aModel)
                {
                    item.ReferenceId = model.Id;
                    item.ReferenceType = (int)ReferenceType.Index.Company;
                    if (item.Id == 0)
                    {
                        item.CreatedById = userId;
                        item.DateCreated = DateTime.Now;
                        _context.Entry(item).State = EntityState.Added;
                    }
                    else
                    {
                        item.ModifiedById = userId;
                        item.DateModified = DateTime.Now;

                        _context.Entry(item).State = EntityState.Modified;
                        _context.Entry(item).Property(m => m.CreatedById).IsModified = false;
                        _context.Entry(item).Property(m => m.DateCreated).IsModified = false;
                    }

                    await _context.SaveChangesAsync();
                }

                var addressIds = aModel.Where(m => m.Id != 0).Select(m => m.Id).ToList();
                var toDeleteAddress = _context.Addresses.Where(m => m.ReferenceId == model.Id && m.ReferenceType == (int)ReferenceType.Index.Company && !addressIds.Contains(m.Id));

                _context.RemoveRange(toDeleteAddress);
                await _context.SaveChangesAsync();

                var logoIds = clModel.Where(m => m.Id != 0).Select(m => m.Id).ToList();
                var toDeleteLogos = _context.CompanyLogos.Where(m => m.CompanyId == model.Id && !logoIds.Contains(m.Id));

                DeleteProfilePictures(model.Id, toDeleteLogos.ToList(), webHostingEnv);

                _context.RemoveRange(toDeleteLogos);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // pending switch to CompanyLogo?
        private void DeleteProfilePictures(int companyId, List<CompanyLogo> logoList, string webHostingEnv)
        {
            try
            {
                var company = _context.Companies.FirstOrDefault(m => m.Id == companyId);

                if (company == null)
                    return;

                foreach (var item in logoList)
                {
                    if (item == null)
                        return;

                    string filePath = string.Format("{0}{1}", webHostingEnv, item.Location.Replace("/", "\\"));

                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Company> CreateAsync(Company module, int userId)
        {
            module.CreatedById = userId;
            module.DateCreated = DateTime.Now;

            string[] exclusions = { "ModifiedById", "DateModified" };
            module = await _contextHelper.CreateAsync(module, exclusions);
            return module;
        }

        public async Task<Company> UpdateAsync(Company module, int userId)
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
                var companyToDelete = _context.Companies.FirstOrDefault(m => m.Id == id);
                if (companyToDelete != null)
                {
                    var logosToDelete = await _context.CompanyLogos.Where(m => m.CompanyId == companyToDelete.Id).ToArrayAsync();
                    if (logosToDelete != null && logosToDelete.Any())
                        await _companyLogo.BatchDeleteAsync(logosToDelete); //batchDeleteAsync from ICompanyLogoRepository

                    await _contextHelper.DeleteAsync(companyToDelete);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            try
            {
                var companyLists = await _context.Companies.Where(c => ids.Contains(c.Id)).ToArrayAsync();

                if (!companyLists.Any())
                    return;

                foreach (var company in companyLists)
                {
                    var logoLists = await _context.CompanyLogos.Where(l => l.CompanyId == company.Id).ToArrayAsync();

                    if (logoLists.Any())
                        await _companyLogo.BatchDeleteAsync(logoLists);
                }

                await _contextHelper.BatchDeleteAsync(companyLists);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Operation
    }
}
