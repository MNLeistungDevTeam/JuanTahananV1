CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalAppStatusAndStage]

AS
	SET NOCOUNT ON;

	SELECT
		COUNT(CASE WHEN apl.ApprovalStatus IN (0,1,2,3,5,11) THEN 1 END) AS CreditVerifStage,
		COUNT(CASE WHEN apl.ApprovalStatus IN (4, 6, 7, 9, 10) THEN 1 END) AS AppCompletionStage,
		COUNT(CASE WHEN apl.ApprovalStatus = 8 THEN 1 END) AS PostApprovalStage,
		COUNT(CASE WHEN apl.ApprovalStatus IN (2, 9) THEN 1 END) AS TotalDeferred,
		COUNT(CASE WHEN apl.ApprovalStatus IN (3, 4, 7, 8) THEN 1 END) AS TotalApproved
	FROM ApplicantsPersonalInformation apl
	
RETURN 0