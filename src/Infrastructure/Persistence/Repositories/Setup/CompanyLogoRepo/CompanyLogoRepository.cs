using DMS.Application.Interfaces.Setup.CompanyLogoRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.CompanyDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.CompanyLogoRepo
{
    public class CompanyLogoRepository : ICompanyLogoRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<CompanyLogo> _contextHelper;
        private readonly ISQLDatabaseService _db;

        public CompanyLogoRepository(
            DMSDBContext context, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<CompanyLogo>(_context);
            _db = db;
        }

        public async Task<CompanyLogo?> GetByIdAsync(int id)
        {
            var data = await _contextHelper.GetByIdAsync(id);

            return data;
        }

        public async Task<List<CompanyLogo>> GetByCompanyIdAsync(int id)
        {
            var data = await _context.CompanyLogos.Where(cl => cl.CompanyId == id).ToListAsync();

            return data;
        }

        public async Task<IEnumerable<CompanyLogoModel>> GetByCompanyId(int companyId)
        {
            var data = await _db.LoadDataAsync<CompanyLogoModel, dynamic>("spCompanyLogo_GetAllByCompanyId", new { companyId });
            return data;
        }

        public async Task<CompanyLogoModel?> GetByDesc(int loanPurposeId, string description)
        {
            var data = await _db.LoadSingleAsync<CompanyLogoModel, dynamic>("spCompanyLogo_GetByDesc", new { loanPurposeId, description });
            return data;
        }

        public async Task<List<CompanyLogo>> GetAllAsync()
        {
            var data = await _contextHelper.GetAllAsync();

            return data;
        }

        public async Task<CompanyLogo> SaveAsync(CompanyLogo loanPurposeLogo, int userId)
        {
            if (loanPurposeLogo.Id == 0)
            {
                loanPurposeLogo = await CreateAsync(loanPurposeLogo, userId);
            }
            else
            {
                loanPurposeLogo = await UpdateAsync(loanPurposeLogo, userId);
            }

            return loanPurposeLogo;
        } 

        public async Task<CompanyLogo> CreateAsync(CompanyLogo loanPurposeLogo, int createdById)
        {
            loanPurposeLogo.CreatedById = createdById;
            loanPurposeLogo.DateCreated = DateTime.Now;
            var result = await _contextHelper.CreateAsync(loanPurposeLogo, "ModifiedById", "DateModified");

            return result;
        }

        public async Task<CompanyLogo> UpdateAsync(CompanyLogo loanPurposeLogo, int modifiedById)
        {
            loanPurposeLogo.ModifiedById = modifiedById;
            loanPurposeLogo.DateModified = DateTime.Now;
            var result = await _contextHelper.UpdateAsync(loanPurposeLogo, "CreatedById", "DateCreated");

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.CompanyLogos.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.CompanyLogos.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(List<CompanyLogo> loanPurposeLogos)
        {
            await _contextHelper.BatchDeleteAsync(loanPurposeLogos);
        }
    }
}
