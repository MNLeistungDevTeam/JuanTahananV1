CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalInfo]

	@userId INT,
	@companyId INT,
	@developerId INT 

AS
 SELECT
	-- Get the count of drafts
	COUNT(CASE WHEN api.ApprovalStatus = 0 THEN 1 END) AS TotalPendingReview,
	-- Get the count of Submitted
	COUNT(CASE WHEN api.ApprovalStatus IN (1,6) THEN 1 END) AS TotalSubmitted,
	-- Get the count of Pagibig/Developer approved
	COUNT(CASE WHEN api.ApprovalStatus IN (3,4,7,8) THEN 1 END) AS TotalApprove,
	-- Get the count of deferred
	COUNT(CASE WHEN api.ApprovalStatus = 2 OR api.ApprovalStatus = 9 THEN 1 END) AS TotalDisApprove
FROM
    ApplicantsPersonalInformation api
LEFT JOIN BeneficiaryInformation bi ON bi.UserId = api.UserId

    WHERE  1 = (
		CASE 
			WHEN @userId IS NULL THEN 1
			WHEN @userId = 0 THEN 1
			WHEN @userId IS NOT NULL AND @userId = api.UserId THEN 1
		END
	) AND api.CompanyId  = @companyId
	AND   1 = (
		CASE 
			WHEN @developerId IS NULL THEN 1
			WHEN @developerId IS NOT NULL AND @developerId = bi.PropertyDeveloperId THEN 1
		END
	) 

RETURN 0