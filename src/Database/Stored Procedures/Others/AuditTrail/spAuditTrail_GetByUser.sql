CREATE PROCEDURE [dbo].[spAuditTrail_GetByUser]
	@userId INT,
	@dateFrom DATETIME,
	@dateTo DATETIME
AS
BEGIN
	SET @dateTo = CONVERT(varchar, @dateTo, 101) + ' 23:59:59'

	SELECT 
		[at].*,
		u.UserName,
		CONCAT(u.LastName, ' ', u.FirstName) FullName,
		IIF(aur.ColumnDisplay IS NULL, [at].ColumnName, aur.ColumnDisplay) ColumnName2,
		[at].OldValue OldDescription,
		[at].NewValue NewDescription
	FROM AuditTrail [at]
	LEFT JOIN [User] u ON u.Id = [at].UserId
	LEFT JOIN AuditTrailRef aur ON aur.ColumnName = [at].ColumnName

	WHERE 
		UserId = @userId AND 
		[ChangeDate] >= COALESCE(@dateFrom, [ChangeDate]) AND
		[ChangeDate] <= COALESCE(@dateTo, [ChangeDate])
	ORDER BY ChangeDate DESC
END