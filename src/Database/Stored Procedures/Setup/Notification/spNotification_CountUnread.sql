 CREATE PROCEDURE [dbo].[spNotification_CountUnread]
 @userId INT,
 @companyId INT
 AS
	SELECT COUNT (*) AS UnreadNotif FROM NotificationReceiver nr
	LEFT JOIN [Notification] n ON n.Id = nr.NotifId
	where nr.ReceiverId = @userId and nr.IsRead = 0 AND nr.IsDelete = 0 AND n.CompanyId = @companyId
RETURN 0 



 