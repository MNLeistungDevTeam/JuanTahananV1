CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetInq]
	@companyId INT
AS
	
	SET NOCOUNT ON;

	SELECT

		COUNT(bc.Id) AS TotalBCF,

		COUNT(CASE WHEN bc.ApprovalStatus = 1 THEN 1 END) AS TotalSubmitted,

		COUNT(CASE WHEN bc.ApprovalStatus IN (3,4,7,8) THEN 1 END) AS TotalQualified,

		COUNT(CASE WHEN bc.ApprovalStatus IN (2,9) THEN 1 END) AS TotalReturned

	FROM BuyerConfirmation bc WHERE bc.CompanyId = @companyId

RETURN 0
