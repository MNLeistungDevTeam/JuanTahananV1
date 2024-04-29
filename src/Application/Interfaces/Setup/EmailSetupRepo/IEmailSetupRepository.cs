using DevExpress.Entity.Model.Metadata;
using DMS.Application.Services;
using DMS.Domain.Dto.EmailSetupDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.EmailSetupRepo
{
    public interface IEmailSetupRepository
    {
        Task<EmailSetupModel?> GetByCompany(int companyId);
        Task<IEnumerable<EmailSetupModel>> GetList(int companyId);
        Task<EmailSetup> SaveAsync(EmailSetupModel mailLog, int userId);
        Task BatchDeleteAsync(int[] ids);
        Task<EmailSetup> CreateAsync(EmailSetup EmailSetup, int userId);
        Task DeleteAsync(EmailSetup EmailSetup);
        Task DeleteAsync(int id);
        Task<List<EmailSetup>> GetAll();
        Task<EmailSetup?> GetById(int id);
        Task<List<EmailSetup>> GetByIds(int[] ids);
        Task<EmailSetup> UpdateAsync(EmailSetup EmailSetup, int userId);
    }
}
