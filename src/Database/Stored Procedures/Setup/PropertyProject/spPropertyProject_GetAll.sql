CREATE PROCEDURE [dbo].[spPropertyProject_GetAll]
	 
AS
	SELECT  pp.*,c.[Name] CompanyName From PropertyProject pp 
	LEFT JOIN Company c ON c.Id = pp.CompanyId
RETURN 0
