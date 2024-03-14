CREATE PROCEDURE [dbo].[spNotification_GetByUserId]
 	@userId INT,
	@isRead BIT,
	@isDelete BIT,
	@companyId INT
AS
SELECT  
	notifrcv.Id,
	notifrcv.NotifId,
	n.Title,
	n.Content,
	n.Preview,
	n.ActionLink,
	n.PriorityLevel,
	pl.LevelName,
	n.DateCreated,
	n.SenderId,
	u.FirstName as SenderName,
	notifrcv.ReceiverType,
	notifrcv.ReceiverId,
	CASE WHEN notifrcv.ReceiverType = 1 THEN 'Role'
		WHEN notifrcv.ReceiverType = 2 THEN 'User'
		ELSE 'Approval'
	END ReceiverTypeName,
	notifrcv.IsRead,
	notifrcv.IsDelete
FROM NotificationReceiver notifrcv
LEFT JOIN [Notification] n ON n.Id = notifrcv.NotifId
LEFT JOIN NotificationPriorityLevel pl ON pl.Id = n.PriorityLevel
LEFT JOIN [User] u ON u.Id =  n.SenderId
WHERE 
	notifrcv.ReceiverId = COALESCE(@userId, notifrcv.ReceiverId) AND 
	1 = (
			CASE
				WHEN @isRead IS NULL THEN 1
				WHEN @isRead IS NOT NULL AND notifrcv.IsRead = @isRead THEN 1
			END
	) AND
	1 = (
		CASE
			WHEN @isDelete IS NULL THEN 1
			WHEN @isDelete IS NOT NULL AND notifrcv.IsDelete = @isDelete THEN 1
		END
	) 
	AND n.CompanyId = @companyId

ORDER BY n.DateCreated DESC;
RETURN 0 
