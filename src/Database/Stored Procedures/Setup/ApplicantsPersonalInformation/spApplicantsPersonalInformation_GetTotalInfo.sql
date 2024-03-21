
CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalInfo]

@userId INT
AS
 SELECT
    COUNT(CASE WHEN ApprovalStatus = 1 THEN 1 END) OVER () AS TotalPendingReview,
    COUNT(CASE WHEN ApprovalStatus = 2 THEN 1 END) OVER () AS TotalApprove,
    COUNT(CASE WHEN ApprovalStatus = 3 THEN 1 END) OVER () AS TotalDisApprove
FROM
    ApplicantsPersonalInformation

    WHERE  1 = (
	CASE WHEN @userId IS NULL THEN 1
	WHEN @userId = 0 THEN 1
	WHEN @userId IS NOT NULL AND @userId = UserId THEN 1
	END
	)

RETURN 0
