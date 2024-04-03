CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalInfo]

@userId INT

AS
 SELECT
	--Get submitted/pending review count
    COUNT(CASE WHEN ApprovalStatus = 1 THEN 1 END) OVER () AS TotalPendingReview,
	--get pagibig verified
    COUNT(CASE WHEN ApprovalStatus = 4 THEN 1 END) OVER () AS TotalApprove,
	--get defered count
    COUNT(CASE WHEN ApprovalStatus = 2 THEN 1 END) OVER () AS TotalDisApprove
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