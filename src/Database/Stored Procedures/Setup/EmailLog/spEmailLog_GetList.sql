
 CREATE PROCEDURE [dbo].[spEmailLog_GetList]
	 
AS
	SELECT e.*, 
	IIF(e.ReceiverId <> 0, u1.FirstName, '') ReceiverName,
	CONCAT(u.LastName, ' , ', u.FirstName, ' ', u.MiddleName ) SenderName
	FROM EmailLog e
	LEFT JOIN [User] u1 on u1.Id = ReceiverId
	LEFT JOIN [User] u on u.Id = e.SenderId
	ORDER BY e.Id DESC
RETURN 0
