CREATE PROCEDURE [dbo].[spForm2Page_GetByApplicantId]
@applicantId INT
AS
	SELECT * FROM Form2Page WHERE ApplicantsPersonalInformationId = @applicantId
RETURN 0
