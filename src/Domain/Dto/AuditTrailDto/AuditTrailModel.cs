namespace DMS.Domain.Dto.AuditTrailDto;

public class AuditTrailModel
{
    public int Id { get; set; }
    public int RecordPK { get; set; }
    public string? Action { get; set; }
    public string? TableName { get; set; }
    public string? ColumnName { get; set; }
    public string? ColumnName2 { get; set; }
    public string? OldValue { get; set; }
    public string? OldDescription { get; set; }
    public string? NewValue { get; set; }
    public string? NewDescription { get; set; }
    public DateTime ChangeDate { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }

    #region Display Properties

    public string? FirstName { get; set; }
    public string? TransactionNo { get; set; }

    #endregion Display Properties
}