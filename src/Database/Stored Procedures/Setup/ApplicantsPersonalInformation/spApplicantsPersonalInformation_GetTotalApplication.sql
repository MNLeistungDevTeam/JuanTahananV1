CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalApplication]
	@roleId int
AS
	SET NOCOUNT ON;

	SELECT
		-- Get the count of new application
		COUNT(CASE WHEN apl.ApprovalStatus IN (1) THEN 1 END) AS NewApplication,
		-- Get the count of needs developer approval
		COUNT(CASE WHEN apl.ApprovalStatus IN (1,6) THEN 1 END) AS NeedsDeveloperApproval,
		-- Get the count of needs pagibig approval
		COUNT(CASE WHEN apl.ApprovalStatus IN (3,7) THEN 1 END) AS NeedsPagibigApproval,
		-- Get the count of ready for approval
		COUNT(CASE WHEN apl.ApprovalStatus = 8  THEN 1 END) AS ReadyForApproval,
		-- Get the count of credit verification
		COUNT(CASE WHEN apl.ApprovalStatus IN (0,1,2,3,5,11)  THEN 1 END) AS CreditVerification,
		-- Get the count of application completion
		COUNT(CASE WHEN apl.ApprovalStatus IN  (4,6,7,9,10)   THEN 1 END) AS ApplicationCompletion,
		-- Get the count of Post approval
		COUNT(CASE WHEN apl.ApprovalStatus IN (8)  THEN 1 END) AS PostApproval,
		-- Get the coun of Application in Credit Verif
		COUNT(CASE WHEN apl.ApprovalStatus IN (0,1,2,3,5,11) THEN 1 END) AS CreditVerifStage
  
	FROM ApplicantsPersonalInformation apl
	LEFT JOIN [User] u ON u.Id = apl.UserId
	LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
	LEFT JOIN [Role] r ON r.Id = ur.RoleId
RETURN 0
