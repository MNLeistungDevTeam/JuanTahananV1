CREATE PROCEDURE [dbo].[spProject_GetByCompanyId]
	@companyId INT
 
AS
	SELECT  pp.*,c.[Name] CompanyName From PropertyProject pp 
	LEFT JOIN Company c ON c.Id = pp.CompanyId
	WHERE pp.CompanyId = @companyId
RETURN 0
