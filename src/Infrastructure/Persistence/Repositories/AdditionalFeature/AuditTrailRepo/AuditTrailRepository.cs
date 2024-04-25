using DMS.Application.Interfaces.AdditionalFeature.AudtiTrailRepo;
using DMS.Application.Services;
using DMS.Domain.Dto.AuditTrailDto;

namespace DMS.Infrastructure.Persistence.Repositories.AdditionalFeature.AuditTrailRepo;

public class AuditTrailRepository : IAuditTrailRepository
{
    private readonly ISQLDatabaseService _db;

    public AuditTrailRepository(ISQLDatabaseService db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AuditTrailModel>> GetAuditTrail(int recordId, string type)
    {
        var data = await _db.LoadDataAsync<AuditTrailModel, dynamic>("", new { recordId, type });
        return data;
    }

    public async Task<IEnumerable<AuditTrailModel>> GetAuditTrailByUser(int userId, DateTime? dateFrom, DateTime? dateTo)
    {
        var data = await _db.LoadDataAsync<AuditTrailModel, dynamic>("spAuditTrail_GetByUser", new { userId, dateFrom, dateTo });
        return data;
    }


    public async Task<IEnumerable<AuditTrailModel>> GetAuditTrailTransactions(string transactionNo)
    {
        var data = await _db.LoadDataAsync<AuditTrailModel, dynamic>("spAuditTrail_GetAuditTrailTransactions", new { transactionNo });
        return data;
    }
}