using DMS.Application.PredefinedReports;
using DMS.Application.PredefinedReports.HousingLoanApplication;
using DMS.Domain.Entities;

namespace DMS.Application.Services
{
    public interface IReportsService
    {
        Task<PredefinedReports.BuyerConfirmation.BuyerConfirmation> GenerateBuyerConfirmationForm(string? buyerConfirmationCode, string? rootFolder);
        Task<LoanApplicationForm> GenerateHousingLoanForm(string? applicationCode, string? rootFolder);

    }
}