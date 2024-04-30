namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationInfoModel
{
    public decimal TotalApplication { get; set; }
    public decimal TotalSubmitted { get; set; }
    public decimal TotalApprove { get; set; }
    public decimal TotalDisApprove { get; set; }
    public decimal TotalWithdrawn { get; set; }

    //Developer & Pagibig Cards

    public decimal NewApplication { get; set; }
    public decimal NeedsDeveloperApproval { get; set; }
    public decimal NeedsPagibigApproval { get; set; }
    public decimal ReadyForApproval { get; set; }
    public decimal CreditVerification { get; set; }
    public decimal ApplicationCompletion { get; set; }
    public decimal ReadyPostApp { get; set; }

    //Credit Verif & App Comple Chart

    public decimal ApplicationInDraft { get; set; }
    public decimal Submitted { get; set; }
    public decimal DeveloperVerified { get; set; }
    public decimal PagibigVerified { get; set; }
    public decimal PagibigDeferred { get; set; }
    public decimal DeveloperDeferred { get; set; }
    public decimal Withdrawn { get; set; }

    //Status Chart

    public decimal TotalApproved { get; set; }
    public decimal TotalDeferred { get; set; }
}