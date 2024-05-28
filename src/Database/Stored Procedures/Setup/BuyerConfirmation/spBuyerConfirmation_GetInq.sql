CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetInq]
	@companyId INT,
	@developerId INT
AS
	
	SET NOCOUNT ON;

	SELECT

		COUNT(bc.Id) AS TotalBCF,

		COUNT(CASE WHEN bc.ApprovalStatus = 1 THEN 1 END) AS TotalSubmitted,

		COUNT(CASE WHEN bc.ApprovalStatus IN (3,4,7,8) THEN 1 END) AS TotalQualified,

		COUNT(CASE WHEN bc.ApprovalStatus IN (2,9) THEN 1 END) AS TotalReturned

	FROM BuyerConfirmation bc 
	LEFT JOIN BeneficiaryInformation bi ON bi.UserId = bc.UserId
	WHERE 1 = (	CASE WHEN @developerId IS NULL THEN 1 
					 WHEN @developerId IS NOT NULL AND @developerId = bi.PropertyDeveloperId THEN 1
			END) 
			AND  bc.CompanyId = @companyId
	

RETURN 0
