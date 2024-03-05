using DMS.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;

namespace DMS.Application.Interfaces.Setup;

public interface IReportDbContext
{
    void InitializeDatabase();

    DbSet<JsonDataConnectionDescription> JsonDataConnections { get; set; }
    DbSet<ReportItem> Reports { get; set; }
    DbSet<SqlDataConnectionDescription> SqlDataConnections { get; set; }
}