CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_Get]
	@id INT
AS
	SELECT  * FROM ApplicantsPersonalInformation WHERE Id = @id
RETURN 0
