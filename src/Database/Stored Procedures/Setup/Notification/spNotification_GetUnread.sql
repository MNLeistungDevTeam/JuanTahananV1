CREATE PROCEDURE [dbo].[spNotification_GetUnread]
@userId INT,
@pageNumber INT,
@pageSize INT,
@type INT,
@companyId INT
AS

 
 SET @type = ISNULL(@type, 0); 

WITH NotificationData AS (
    SELECT  
		n.NotificationType,
        nr.Id,
        nr.NotifId,
        n.Title,
        n.Content,
        n.Preview,
        n.ActionLink,
        n.PriorityLevel,
        npl.LevelName,
        n.DateCreated,
        n.SenderId,
        u.FirstName SenderName,
        nr.ReceiverType,
        nr.ReceiverId,
		CASE WHEN nr.ReceiverType = 1 THEN 'Role'
		 WHEN nr.ReceiverType = 2 THEN 'User'
		 WHEN nr.ReceiverType = 3 THEN 'Approval'
		END ReceiverTypeName,
        nr.IsRead,
        nr.IsDelete
    FROM NotificationReceiver nr
    LEFT JOIN [Notification] n ON n.Id = nr.NotifId And n.CompanyId = @companyId 
    LEFT JOIN NotificationPriorityLevel npl ON npl.Id = n.PriorityLevel
    LEFT JOIN [User] u ON u.Id = n.SenderId
    WHERE nr.ReceiverId = @userId AND nr.IsDelete = 0  
	 AND (
        (@type = 3 AND NotificationType = @type)
        OR (@type != 3 AND NotificationType != 3)
    )
)
SELECT *,
		PageNumberLimit = CEILING(CAST((SELECT COUNT(*) FROM NotificationData) AS FLOAT) / @pageSize)
FROM NotificationData 
ORDER BY DateCreated DESC
OFFSET (@pageNumber - 1) * @pageSize ROWS
FETCH NEXT @pageSize ROWS ONLY;
RETURN 0


 