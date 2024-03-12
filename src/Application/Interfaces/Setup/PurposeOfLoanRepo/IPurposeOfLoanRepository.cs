using DMS.Domain.Dto.PurposeOfLoanDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.Setup.PurposeOfLoanRepo
{
    public interface IPurposeOfLoanRepository
    {
        Task<IEnumerable<PurposeOfLoanModel>> GetAllAsync();
        Task<PurposeOfLoanModel?> GetAsync(int id);
    }
}
