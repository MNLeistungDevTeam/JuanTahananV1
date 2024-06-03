using DMS.Application.Interfaces.AdditionalFeature.LockedTransactionRepo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace DMS.Web.Controllers.AdditionalFeatures
{
    public class TransactionController : Controller
    {
        private readonly ILockedTransactionRepository _transactionLockRepo;

        public TransactionController(ILockedTransactionRepository transactionLockRepo)
        {
            _transactionLockRepo = transactionLockRepo;
        }

        [Route("[controller]/UpdateLockStatus/{transactionNo}")]
        public async Task<IActionResult> UpdateLockStatus(string transactionNo)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var result = await _transactionLockRepo.UpdateLockedTransaction(userId, transactionNo);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}