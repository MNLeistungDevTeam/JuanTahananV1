CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetByUserId]
@userId INT
AS
	SELECT * FROM BuyerConfirmation WHERE UserId = @userId
RETURN 0
