using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.UserCompanyRepo
{
    public interface IUserCompanyRepository
    {
        Task SaveAsync(int? developerId, int userId, int editorId);
    }
}
