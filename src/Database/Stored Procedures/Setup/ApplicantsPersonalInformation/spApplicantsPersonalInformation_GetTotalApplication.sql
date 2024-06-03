CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalApplication]
	@roleId INT,
	@companyId INT,
	@developerId INT
AS
	SET NOCOUNT ON;

	SELECT
		-- Get the count of new application
		COUNT(CASE WHEN apl.ApprovalStatus = 1 THEN 1 END) AS NewApplication,

		-- Get the count of needs developer approval
		COUNT(CASE WHEN apl.ApprovalStatus IN (1,6) THEN 1 END) AS NeedsDeveloperApproval,

		-- Get the count of needs pagibig approval
		COUNT(CASE WHEN apl.ApprovalStatus IN (3,7) THEN 1 END) AS NeedsPagibigApproval,

		-- Get the count of credit verification & credit verification stage
		COUNT(CASE WHEN apl.ApprovalStatus IN (0,1,2,3,5,11)  THEN 1 END) AS CreditVerification,

		-- Get the count of application completion & application completion stage
		COUNT(CASE WHEN apl.ApprovalStatus IN  (4,6,7,9,10)   THEN 1 END) AS ApplicationCompletion,

		-- Get the count of Post approval, Ready for approval, and post approval stage
		COUNT(CASE WHEN apl.ApprovalStatus = 8 THEN 1 END) AS ReadyPostApp,

		-- Get the count of deffered application
		COUNT(CASE WHEN apl.ApprovalStatus IN (2, 9) THEN 1 END) AS TotalDeferred,

		--Get the count of approved application
		COUNT(CASE WHEN apl.ApprovalStatus IN (3, 4, 7, 8) THEN 1 END) AS TotalApproved

	FROM ApplicantsPersonalInformation apl
	LEFT JOIN [User] u ON u.Id = apl.UserId
	LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
	LEFT JOIN [Role] r ON r.Id = ur.RoleId
	LEFT JOIN [BeneficiaryInformation] bi ON bi.UserId = apl.UserId
	WHERE apl.CompanyId = @companyId 
	AND 1 = (CASE WHEN @developerId IS NULL THEN 1
	WHEN @developerId IS NOT NULL AND @developerId = bi.PropertyDeveloperId THEN 1
	END)
RETURN 0
