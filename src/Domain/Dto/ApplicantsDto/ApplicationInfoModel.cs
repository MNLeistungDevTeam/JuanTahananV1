namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationInfoModel
{
    public decimal TotalApplication { get; set; }
    public decimal TotalSubmitted { get; set; }
    public decimal TotalApprove { get; set; }
    public decimal TotalDisApprove { get ; set; }
    public decimal TotalWithdrawn { get; set; }
}