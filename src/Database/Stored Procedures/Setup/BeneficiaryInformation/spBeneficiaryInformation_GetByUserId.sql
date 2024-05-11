CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetByUserId]
	@userId INT
AS
	SELECT  * FROM BeneficiaryInformation WHERE UserId = @userId
RETURN 0
