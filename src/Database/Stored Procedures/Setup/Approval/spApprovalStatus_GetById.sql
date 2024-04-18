CREATE PROCEDURE [dbo].[spApprovalStatus_GetById]
@id INT
AS
	SELECT  * FROM ApprovalStatus where Id = @id
RETURN 0
