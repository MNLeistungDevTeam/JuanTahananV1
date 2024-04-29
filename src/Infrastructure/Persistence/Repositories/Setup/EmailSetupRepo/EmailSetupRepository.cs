using AutoMapper;
using DMS.Application.Interfaces.Setup.EmailSetupRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EmailSetupDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.EmailSetupRepo
{
    public class EmailSetupRepository : IEmailSetupRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<EmailSetup> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public EmailSetupRepository(DMSDBContext context, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<EmailSetup>(context);
            _mapper = mapper;
            _db = db;
        }
        #region Getter
        public async Task<EmailSetup?> GetById(int id)
        {
            var result = await _contextHelper.GetByIdAsync(id);
            return result;
        }

        public async Task<List<EmailSetup>> GetByIds(int[] ids)
        {
            var result = await _context.EmailSetups.Where(d => ids.Contains(d.Id)).ToListAsync();
            return result;
        }

        public async Task<List<EmailSetup>> GetAll()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        public async Task<IEnumerable<EmailSetupModel>> GetList(int companyId) =>
            await _db.LoadDataAsync<EmailSetupModel, dynamic>("spEmailSetup_GetByCompany", new { companyId });

        public async Task<EmailSetupModel?> GetByCompany(int companyId) =>
            await _db.LoadSingleAsync<EmailSetupModel, dynamic>("spEmailSetup_GetByCompany", new { companyId });


        #endregion

        #region Operations
        public async Task<EmailSetup> SaveAsync(EmailSetupModel setup, int userId)
        {
            var _emailSetup = _mapper.Map<EmailSetup>(setup);
            if (_emailSetup.Id == 0)
            {
                var checker = await GetList(setup.CompanyId);
                if (checker.Any() && checker.Count() > 0)
                {
                    throw new Exception("This company has been using default email setup");
                }
                else
                {
                    _emailSetup = await CreateAsync(_emailSetup, userId);
                }
            }
            else
            {
                _emailSetup = await UpdateAsync(_emailSetup, userId);
            }

            return _emailSetup;
        }

        public async Task<EmailSetup> CreateAsync(EmailSetup EmailSetup, int userId)
        {
            EmailSetup.CreatedById = userId;
            EmailSetup.DateCreated = DateTime.Now;
            var result = await _contextHelper.CreateAsync(EmailSetup);

            return result;
        }

        public async Task<EmailSetup> UpdateAsync(EmailSetup EmailSetup, int userId)
        {
            EmailSetup.ModifiedById = userId;
            EmailSetup.DateModified = DateTime.Now;
            string[] exclusions = { "CreatedById", "DateCreated" };
            var result = await _contextHelper.UpdateAsync(EmailSetup, exclusions);

            return result;
        }

        public async Task DeleteAsync(EmailSetup EmailSetup)
        {
            await _contextHelper.DeleteAsync(EmailSetup);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.EmailSetups.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.EmailSetups.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }
        #endregion

        #region Private Helper

        #endregion
    }
}
