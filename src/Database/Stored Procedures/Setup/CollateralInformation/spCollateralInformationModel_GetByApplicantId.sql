CREATE PROCEDURE [dbo].[spCollateralInformationModel_GetByApplicantId]
@applicantId INT
AS
	SELECT * FROM CollateralInformation WHERE ApplicantsPersonalInformationId = @applicantId
RETURN 0
