CREATE PROCEDURE [dbo].[spAuditTrail_Get]
	@recordId NVARCHAR(500),
	@type NVARCHAR(50)
AS
BEGIN
	SELECT 
		[at].*,
		u.UserName,
		CONCAT(u.LastName, ' ', u.FirstName) FullName,
		IIF(aur.ColumnDisplay IS NULL, [at].ColumnName, aur.ColumnDisplay) ColumnName2,
		CASE 
			WHEN t1.Id IS NOT NULL THEN t1.[Name] 
			ELSE [at].OldValue 
		END OldDescription,
		CASE 
			WHEN t2.Id IS NOT NULL THEN t2.[Name] 
			ELSE [at].NewValue 
		END NewDescription
	FROM AuditTrail [at]
	LEFT JOIN [User] u ON u.Id = [at].UserId
	LEFT JOIN AuditTrailRef aur ON aur.ColumnName = [at].ColumnName

	WHERE RecordPk = @recordId AND TableName = @type
	ORDER BY ChangeDate DESC
END
