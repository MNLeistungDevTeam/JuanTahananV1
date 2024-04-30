CREATE PROCEDURE [dbo].[spEmailSetup_GetByCompany]
@companyId INT
AS
	SELECT 
	ems.*, c.Name CompanyName
	FROM EmailSetup ems
	LEFT JOIN Company c ON c.Id = ems.CompanyId
	WHERE ems.CompanyId = @companyId
RETURN 0
