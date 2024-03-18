CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalInfo]
AS
 SELECT
    COUNT(CASE WHEN ApprovalStatus = 1 THEN 1 END) OVER () AS TotalPendingReview,
    COUNT(CASE WHEN ApprovalStatus = 2 THEN 1 END) OVER () AS TotalApprove,
    COUNT(CASE WHEN ApprovalStatus = 3 THEN 1 END) OVER () AS TotalDisApprove
FROM
    ApplicantsPersonalInformation;

RETURN 0
