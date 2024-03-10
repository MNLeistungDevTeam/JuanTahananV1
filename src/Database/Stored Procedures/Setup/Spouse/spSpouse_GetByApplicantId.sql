CREATE PROCEDURE [dbo].[spSpouse_GetByApplicantId]
@applicantId INT
AS
	SELECT * FROM Spouse WHERE ApplicantsPersonalInformationId = @applicantId
RETURN 0
