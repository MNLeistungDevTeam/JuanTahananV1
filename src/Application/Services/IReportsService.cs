using DMS.Application.PredefinedReports;
using DMS.Application.PredefinedReports.HousingLoanApplication;
using DMS.Domain.Dto.BuyerConfirmationDto;
using DMS.Domain.Dto.ReportDto;
using DMS.Domain.Entities;

namespace DMS.Application.Services
{
    public interface IReportsService
    {
        Task<PredefinedReports.BuyerConfirmation.BuyerConfirmation> GenerateBuyerConfirmationForm(string? buyerConfirmationCode, string? rootFolder);

        Task<byte[]> GenerateBuyerConfirmationFormB64ForDeveloper(BuyerConfirmationModel model, string? rootFolder);

        Task<byte[]> GenerateBuyerConfirmationPDF(ApplicantInformationReportModel aplicantInfoModel, string? rootFolder);

        Task<LoanApplicationForm> GenerateHousingLoanForm(string? applicationCode, string? rootFolder);

        Task<byte[]> GenerateHousingLoanPDF(ApplicantInformationReportModel aplicantInfoModel, string? rootFolder);
    }
}