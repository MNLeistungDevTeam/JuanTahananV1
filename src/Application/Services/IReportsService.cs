using DMS.Application.PredefinedReports.HousingLoanApplication;
using DMS.Domain.Dto.ReportDto;

namespace DMS.Application.Services
{
    public interface IReportsService
    {
        Task<byte[]> GenerateBuyerConfirmationFormNoCode(ApplicantInformationReportModel aplicantInfoModel, string? rootFolder);
        Task<LoanApplicationForm> GenerateHousingLoanForm(string? applicationCode, string? rootFolder);

       // Task<MemoryStream> GenerateHousingLoanFormNoCode(ApplicantInformationReportModel aplicantInfoModel, string? rootFolder);
        Task<byte[]> GenerateHousingLoanFormNoCode(ApplicantInformationReportModel aplicantInfoModel, string? rootFolder);
    }
}