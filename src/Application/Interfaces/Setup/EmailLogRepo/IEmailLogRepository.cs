using DMS.Domain.Dto.EmailLogDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.EmailLogRepo
{
    public interface IEmailLogRepository
    {
        Task<IEnumerable<EmailLogModel>> GetEmailLogList();
        Task<EmailLog> SaveEmailAsync(EmailLogModel mailLog, int userId);

        Task BatchDeleteAsync(int[] ids);

        Task<EmailLog> CreateAsync(EmailLog EmailLog, int userId);

        Task DeleteAsync(EmailLog EmailLog);

        Task DeleteAsync(int id);

        Task<List<EmailLog>> GetAll();

        Task<EmailLog?> GetById(int id);

        Task<List<EmailLog>> GetByIds(int[] ids);

        Task<EmailLog> SaveAsync(EmailLog EmailLog, int userId);

        Task<EmailLog> UpdateAsync(EmailLog EmailLog);
    }
}
