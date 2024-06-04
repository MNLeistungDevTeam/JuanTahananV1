using DevExpress.Pdf.Interop;
using DMS.Application.Interfaces.AdditionalFeature.LockedTransactionRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.LockedTransactionDto;
using DMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Persistence.Repositories.AdditionalFeature.LockedTransactionRepo
{
    public class LockedTransactionRepository : ILockedTransactionRepository
    {
        private readonly DMSDBContext _context;
        private readonly EfCoreHelper<LockedTransaction> _contextHelper;
        private readonly ISQLDatabaseService _db;

        public LockedTransactionRepository(DMSDBContext context, ISQLDatabaseService db)
        {
            _context = context;
            _contextHelper = new EfCoreHelper<LockedTransaction>(_context);
            _db = db;
        }

        public async Task<LockedTransactionModel?> GetLockedTransaction(string transactionNo) =>
            await _db.LoadSingleAsync<LockedTransactionModel, dynamic>("spLockedTransaction_Get", new { transactionNo });

        public async Task<string> UpdateLockedTransaction(int userId, string transactionNo)
        {
            try
            {
                var lockedTransaction = await GetLockedTransaction(transactionNo);

                var newLockedTransaction = new LockedTransaction
                {
                    TransactionNo = transactionNo,
                    LastActivity = DateTime.Now,
                    UserId = userId
                };

                if (lockedTransaction is not null)
                {
                    var isLockedByCurrentUser = userId == lockedTransaction.UserId;

                    if (isLockedByCurrentUser)
                    {
                        newLockedTransaction.Id = lockedTransaction.Id;
                    }
                    else
                    {
                        var minutesSinceLastActivity = (DateTime.Now - lockedTransaction.LastActivity).TotalMinutes;
                        var isLockedBySomeoneElse = minutesSinceLastActivity <= 1;

                        if (isLockedBySomeoneElse)
                        {
                            throw new Exception($"Transaction is currently being used by {lockedTransaction.UserName}");
                        }

                        newLockedTransaction.Id = lockedTransaction.Id;
                    }
                }

                await SaveAsync(newLockedTransaction);

                return "Transaction locked";
            }
            catch
            {
                throw;
            }
        }

        public async Task<LockedTransaction> SaveAsync(LockedTransaction lockedTransaction)
        {
            try
            {
                if (lockedTransaction.Id == 0)
                    lockedTransaction = await CreatedAsync(lockedTransaction);
                else
                    lockedTransaction = await UpdateAsync(lockedTransaction);

                return lockedTransaction;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LockedTransaction> CreatedAsync(LockedTransaction lockedTransaction)
        {
            try
            {
                lockedTransaction = await _contextHelper.CreateAsync(lockedTransaction);

                return lockedTransaction;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LockedTransaction> UpdateAsync(LockedTransaction lockedTransaction)
        {
            try
            {
                lockedTransaction = await _contextHelper.UpdateAsync(lockedTransaction);

                return lockedTransaction;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}