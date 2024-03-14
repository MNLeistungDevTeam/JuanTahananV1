CREATE PROCEDURE [dbo].[spNotification_CountUnreadApprovalTransaction]
 @userId INT,
 @companyId INT
 AS
	SELECT
		COUNT (*) AS UnreadNotif
	FROM NotificationReceiver  nr
	LEFT JOIN [Notification] n ON n.Id = nr.NotifId
	WHERE nr.ReceiverId = @userId and nr.IsRead = 0 AND nr.IsDelete = 0 AND n.NotificationType = 3  AND n.CompanyId = @companyId  --Approval
RETURN 0 

