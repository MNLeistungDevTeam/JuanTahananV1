CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetDistinctPropertyDeveloperName]
AS
	SELECT DISTINCT PropertyDeveloperName 
	FROM BeneficiaryInformation 
	WHERE PropertyDeveloperName IS NOT NULL
RETURN 0
