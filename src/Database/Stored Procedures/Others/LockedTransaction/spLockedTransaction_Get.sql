CREATE PROCEDURE [dbo].[spLockedTransaction_Get]
	@transactionNo NVARCHAR(50)
AS
BEGIN
	SELECT 
		lt.*,
		u.UserName
	FROM LockedTransaction lt
	LEFT JOIN [User] u ON u.Id = lt.UserId
	WHERE TransactionNo = @transactionNo
END