CREATE PROCEDURE [dbo].[spCompanyLogo_GetByDesc]
	@companyId INT,
	@description NVARCHAR(MAX)
AS
BEGIN
	SELECT 
		cl.*
	FROM
		CompanyLogo cl
	WHERE
		cl.[Description] = @description
		AND CompanyId = @companyId
END
