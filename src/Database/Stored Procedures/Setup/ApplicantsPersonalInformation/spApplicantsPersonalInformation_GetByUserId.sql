CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetByUserId]
@userId INT
AS
	SELECT  * FROM ApplicantsPersonalInformation WHERE UserId = @userId
RETURN 0
