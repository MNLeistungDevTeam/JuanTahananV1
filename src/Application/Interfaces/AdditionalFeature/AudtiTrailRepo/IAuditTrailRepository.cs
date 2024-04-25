using DMS.Domain.Dto.AuditTrailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Interfaces.AdditionalFeature.AudtiTrailRepo;

public interface IAuditTrailRepository
{
    Task<IEnumerable<AuditTrailModel>> GetAuditTrail(int recordId, string type);
    Task<IEnumerable<AuditTrailModel>> GetAuditTrailByUser(int userId, DateTime? dateFrom, DateTime? dateTo);
    Task<IEnumerable<AuditTrailModel>> GetAuditTrailTransactions(string transactionNo);
}