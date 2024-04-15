namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationInfoModel
{
    public decimal TotalApplication { get; set; }
    public decimal Draft { get; set; }
    public decimal Submitted { get; set; }
    public decimal DeveloperVerified { get; set; }
    public decimal PagIbigVerified { get; set; }
    public decimal Withdrawn { get; set; }
    public decimal PostSubmitted { get; set; }
    public decimal DeveloperConfirmed { get; set; }
    public decimal PagibigConfirmed { get; set; }
    public decimal Disqualified { get; set; }
    public decimal Discontinued { get; set; }
}