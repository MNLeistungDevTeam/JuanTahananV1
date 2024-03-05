using Microsoft.EntityFrameworkCore;
using Template.Domain.ReportEntities;

namespace Template.Application.Interfaces.Setup;

public interface IReportDbContext
{
    void InitializeDatabase();

    DbSet<JsonDataConnectionDescription> JsonDataConnections { get; set; }
    DbSet<ReportItem> Reports { get; set; }
    DbSet<SqlDataConnectionDescription> SqlDataConnections { get; set; }
}