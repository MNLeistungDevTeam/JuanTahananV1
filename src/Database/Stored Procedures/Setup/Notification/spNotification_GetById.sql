CREATE PROCEDURE [dbo].[spNotification_GetById]
@Id int
AS
SELECT 
	notifrcv.Id,
	notifrcv.NotifId,
	n.Title,
	n.Content,
	n.Preview,
	n.ActionLink,
	n.PriorityLevel,
	n.[DateCreated],
	pl.LevelName,
	notifrcv.ReceiverType,
	notifrcv.ReceiverId,
	ReceiverTypeName = iif(notifrcv.ReceiverType = 2,'User','Role')
FROM NotificationReceiver notifrcv
LEFT JOIN [Notification] n on n.Id = notifrcv.NotifId
LEFT JOIN NotificationPriorityLevel pl on pl.Id = n.PriorityLevel
LEFT JOIN [User] u on u.Id = notifrcv.ReceiverId
WHERE notifrcv.Id =@Id;
RETURN 0
