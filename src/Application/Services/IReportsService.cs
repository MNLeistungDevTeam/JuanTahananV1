using DMS.Application.PredefinedReports;
using DMS.Application.PredefinedReports.HousingLoanApplication;

namespace DMS.Application.Services
{
    public interface IReportsService
    {
        Task<LoanApplicationForm> GenerateHousingLoanForm(string? applicationCode, string? rootFolder);
    }
}