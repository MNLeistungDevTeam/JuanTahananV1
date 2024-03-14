CREATE PROCEDURE [dbo].[spNotification_CountUnreadTransaction]
 @userId INT,
 @companyId INT

 AS
	SELECT
		COUNT (*) AS UnreadNotif
	FROM NotificationReceiver  nr
	LEFT JOIN [Notification] n ON n.Id = nr.NotifId
	WHERE nr.ReceiverId = @userId and nr.IsRead = 0 AND nr.IsDelete = 0 AND n.NotificationType Not in (3) AND n.CompanyId = @companyId   --not Approval only roles or user
RETURN 0 
