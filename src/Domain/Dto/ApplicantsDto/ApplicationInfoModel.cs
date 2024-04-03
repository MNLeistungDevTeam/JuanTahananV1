namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationInfoModel
{
    public decimal TotalApplication { get; set; }
    public decimal ApplicationDraft { get; set; }
    public decimal Submitted { get; set; }
    public decimal DeveloperVerified { get; set; }
    public decimal PagIbigVerified { get; set; }
    public decimal Withdrawn { get; set; }
    public decimal Disqualified { get; set; }
    public decimal Deferred { get; set; }
}