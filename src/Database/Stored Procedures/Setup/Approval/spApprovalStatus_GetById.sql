CREATE PROCEDURE [dbo].[spApprovalStatus_GetById]
@id INT
AS
	SELECT  * FROM ApprovalStatus
RETURN 0
