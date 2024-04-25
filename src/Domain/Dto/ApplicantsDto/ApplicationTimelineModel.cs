namespace DMS.Domain.Dto.ApplicantsDto;

public class ApplicationTimelineModel
{
    public string? Code { get; set; }
    public string? ApplicationStatus { get; set; }
    public string? Stage { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? ApprovalStatusNumber { get; set; }
    public int? ApproverRoleId { get; set; }
    public int? NextApprovalStatus {  get; set; }
}