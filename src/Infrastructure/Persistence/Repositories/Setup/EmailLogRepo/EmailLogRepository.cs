using AutoMapper;
using DMS.Application.Interfaces.Setup.EmailLogRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.EmailLogDto;
using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.Setup.EmailLogRepo
{
    public class EmailLogRepository : IEmailLogRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<EmailLog> _contextHelper;
        private readonly IMapper _mapper;
        private readonly ISQLDatabaseService _db;

        public EmailLogRepository(DMSDBContext context, IMapper mapper, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<EmailLog>(context);
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<EmailLogModel>> GetEmailLogList() =>
        await _db.LoadDataAsync<EmailLogModel, dynamic>("spEmailLog_GetList", new { });

        public async Task<EmailLog?> GetById(int id)
        {
            var result = await _contextHelper.GetByIdAsync(id);
            return result;
        }

        public async Task<List<EmailLog>> GetByIds(int[] ids)
        {
            var result = await _context.EmailLogs.Where(d => ids.Contains(d.Id)).ToListAsync();
            return result;
        }

        public async Task<List<EmailLog>> GetAll()
        {
            var result = await _contextHelper.GetAllAsync();
            return result;
        }

        public async Task<EmailLog> SaveAsync(EmailLog EmailLog, int userId)
        {
            if (EmailLog.Id == 0)
            {
                EmailLog = await CreateAsync(EmailLog, userId);
            }
            else
            {
                EmailLog = await UpdateAsync(EmailLog);
            }

            return EmailLog;
        }

        public async Task<EmailLog> SaveEmailAsync(EmailLogModel mailLog, int userId)
        {
            var emaillog = _mapper.Map<EmailLog>(mailLog);
            if (emaillog.Id == 0)
            {
                emaillog = await CreateAsync(emaillog, userId);
            }
            else
            {
                emaillog = await UpdateAsync(emaillog);
            }

            return emaillog;
        }

        public async Task<EmailLog> CreateAsync(EmailLog EmailLog, int userId)
        {
            EmailLog.SenderId = userId;
            EmailLog.Date = DateTime.Now;
            var result = await _contextHelper.CreateAsync(EmailLog);

            return result;
        }

        public async Task<EmailLog> UpdateAsync(EmailLog EmailLog)
        {
            var result = await _contextHelper.UpdateAsync(EmailLog);

            return result;
        }

        public async Task DeleteAsync(EmailLog EmailLog)
        {
            await _contextHelper.DeleteAsync(EmailLog);
        }

        public async Task DeleteAsync(int id)
        {
            var entities = _context.EmailLogs.FirstOrDefault(d => d.Id == id);
            if (entities is not null)
            {
                await _contextHelper.DeleteAsync(entities);
            }
        }

        public async Task BatchDeleteAsync(int[] ids)
        {
            var entities = _context.EmailLogs.Where(d => ids.Contains(d.Id));
            if (entities is not null)
            {
                await _contextHelper.BatchDeleteAsync(entities);
            }
        }
    }
}
