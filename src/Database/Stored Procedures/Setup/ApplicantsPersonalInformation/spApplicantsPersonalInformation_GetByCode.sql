CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetByCode]
	@code NVARCHAR(100)
AS
	SELECT * FROM ApplicantsPersonalInformation WHERE Code = @code
RETURN 0
