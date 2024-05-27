CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetDistinctPropertyDeveloperName]
AS
	SELECT DISTINCT c.[Name] PropertyDeveloperName 
	FROM BeneficiaryInformation  bi
	LEFT JOIN Company c ON c.Id = bi.PropertyDeveloperId
	WHERE c.[Name] IS NOT NULL
RETURN 0
