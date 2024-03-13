using DMS.Application.PredefinedReports;

namespace DMS.Application.Services
{
    public interface IReportsService
    {
        Task<HousingLoanApplicationForm> GenerateHousingLoanForm(int userId);
    }
}