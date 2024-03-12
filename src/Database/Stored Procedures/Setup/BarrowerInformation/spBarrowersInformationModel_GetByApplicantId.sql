CREATE PROCEDURE [dbo].[spBarrowersInformationModel_GetByApplicantId]
@applicantId INT
AS
	SELECT  * FROM BarrowersInformation WHERE ApplicantsPersonalInformationId = @applicantId
RETURN 0
