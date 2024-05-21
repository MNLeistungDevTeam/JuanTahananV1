CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetByPagibigNumber]
	@pagibigNumber NVARCHAR(255) 
AS
	SELECT bi.*,
			u.ProfilePicture,
			pu.[Description] HouseUnitDescription,
			c.[Name] DeveloperName,
			pp.[Name] ProjectName,
			pl.[Name] LocationName
	from BeneficiaryInformation bi 
	LEFT JOIN [User] u ON u.Id = bi.UserId
	LEFT JOIN PropertyUnit pu ON pu.Id = bi.PropertyUnitId
	LEFT JOIN Company c ON c.Id = bi.PropertyDeveloperId
	LEFT JOIN PropertyLocation pl ON pl.Id = bi.PropertyLocationId
	LEFT JOIN PropertyProject pp ON pp.Id = bi.PropertyProjectId
	WHERE bi.PagibigNumber = @pagibigNumber
	
RETURN 0
