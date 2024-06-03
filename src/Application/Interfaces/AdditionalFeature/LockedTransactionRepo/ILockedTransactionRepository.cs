using DMS.Domain.Dto.LockedTransactionDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.AdditionalFeature.LockedTransactionRepo
{
    public interface ILockedTransactionRepository
    {
        Task<LockedTransaction> CreatedAsync(LockedTransaction lockedTransaction);
        Task<LockedTransactionModel?> GetLockedTransaction(string transactionNo);
        Task<LockedTransaction> SaveAsync(LockedTransaction lockedTransaction);
        Task<LockedTransaction> UpdateAsync(LockedTransaction lockedTransaction);
        Task<string> UpdateLockedTransaction(int userId, string transactionNo);
    }
}
