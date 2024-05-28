CREATE PROCEDURE [dbo].[spBarrowersInformationModel_GetByApplicantId]
@applicantId INT
AS
	SELECT  bai.*,
	   c.[Name] PropertyDeveloperName,
		pl.[Name] PropertyLocationName,
		pp.[Name] PropertyProject,
		pu.[Description] PropertyUnitDescription,
		bi.PropertyDeveloperId,
		bi.PropertyProjectId,
		bi.PropertyUnitId,
		bi.PropertyLocationId
		FROM BarrowersInformation bai
	LEFT JOIN ApplicantsPersonalInformation api ON api.Id = bai.ApplicantsPersonalInformationId
	LEFT JOIN BeneficiaryInformation bi ON bi.UserId = api.UserId
	LEFT JOIN PropertyLocation pl ON pl.Id = bi.PropertyLocationId
	LEFT JOIN PropertyProject pp ON pp.Id = bi.PropertyProjectId
	LEFT JOIN PropertyUnit pu ON pu.Id = bi.PropertyUnitId
	LEFT JOIN Company c ON c.Id = bi.PropertyDeveloperId
	WHERE ApplicantsPersonalInformationId = @applicantId
	
RETURN 0
