CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetByUserId]
@userId INT
AS
	SELECT TOP 1  * FROM ApplicantsPersonalInformation WHERE UserId = @userId
	ORDER BY DateCreated DESC
RETURN 0
