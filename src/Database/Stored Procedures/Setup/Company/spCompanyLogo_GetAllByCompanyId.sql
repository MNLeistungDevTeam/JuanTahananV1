CREATE PROCEDURE [dbo].[spCompanyLogo_GetAllByCompanyId]
	@companyId int
AS
BEGIN
	SELECT 
		cl.*
	FROM
		CompanyLogo cl
	WHERE
		cl.CompanyId = @companyId
END