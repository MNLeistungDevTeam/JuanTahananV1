using DMS.Application.Interfaces.AdditionalFeature.AudtiTrailRepo;
using DMS.Domain.Dto.AuditTrailDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Web.Controllers.Setup;

[Authorize]
public class AuditTrailController : Controller
{
    private readonly IAuditTrailRepository _auditTrailRepo;

    public AuditTrailController(IAuditTrailRepository auditTrailRepo)
    {
        _auditTrailRepo = auditTrailRepo;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("[controller]/Details/{transactionNo}")]
    public IActionResult Details(string transactionNo)
    {
        var viewmodel = new AuditTrailModel
        {
            TransactionNo = transactionNo
        };

        return View("Details", viewmodel);
    }

    #region Api

    public async Task<IActionResult> GetAuditTrail(int recordId, string type)
    {
        try
        {
            var data = await _auditTrailRepo.GetAuditTrail(recordId, type);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public async Task<IActionResult> GetAuditTrailByUser(int userId, DateTime? dateFrom, DateTime? dateTo)
    {
        try
        {
            var atbyuser = await _auditTrailRepo.GetAuditTrailByUser(userId, dateFrom, dateTo);
            var limiteditems = atbyuser.Take(10).ToList();

            return Ok(limiteditems);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("[controller]/GetAuditTrailTransaction/{transactionNo}")]
    public async Task<IActionResult> GetAuditTrailTransaction(string transactionNo)
    {
        try
        {
            var data = await _auditTrailRepo.GetAuditTrailTransactions(transactionNo);

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    #endregion Api
}