CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalInfo]

@userId INT

AS
 SELECT
	-- Get the count of drafts
	COUNT(CASE WHEN ApprovalStatus = 0 THEN 1 END) AS TotalPendingReview,
	-- Get the count of Submitted
	COUNT(CASE WHEN ApprovalStatus IN (1,6) THEN 1 END) AS TotalSubmitted,
	-- Get the count of Pagibig/Developer approved
	COUNT(CASE WHEN ApprovalStatus IN (3,4,7,8) THEN 1 END) AS TotalApprove,
	-- Get the count of deferred
	COUNT(CASE WHEN ApprovalStatus = 2 OR ApprovalStatus = 9 THEN 1 END) AS TotalDisApprove
FROM
    ApplicantsPersonalInformation

    WHERE  1 = (
		CASE 
			WHEN @userId IS NULL THEN 1
			WHEN @userId = 0 THEN 1
			WHEN @userId IS NOT NULL AND @userId = UserId THEN 1
		END
	)

RETURN 0