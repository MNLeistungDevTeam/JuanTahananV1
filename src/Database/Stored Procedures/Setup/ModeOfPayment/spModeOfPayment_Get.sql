CREATE PROCEDURE [dbo].[spModeOfPayment_Get]
@id INT
AS
	SELECT  * FROM ModeOfPayment WHERE Id =@id
RETURN 0
