CREATE PROCEDURE [dbo].[spCompany_GetInfo]
	@companyId INT
AS
BEGIN
	SELECT
	(SELECT COUNT(1) 
	FROM PurchaseOrder WHERE CompanyId = @companyId) TotalTransaction,
	(SELECT COUNT(1) 
	FROM PurchaseOrder WHERE CompanyId = @companyId AND [Status] = 1) TotalPending
END