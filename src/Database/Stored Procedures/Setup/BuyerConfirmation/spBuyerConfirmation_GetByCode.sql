CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetByCode]
	@code NVARCHAR(255)
 
AS
	SELECT  * FROM BuyerConfirmation WHERE Code	 = @code
RETURN 0
