CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalCreditVerif]
	@companyId INT,
	@developerId INT
AS
	SET NOCOUNT ON;
	
	SELECT
		-- Get the count of Application in draft
		COUNT(CASE WHEN apl.ApprovalStatus = 0 THEN 1 END) AS ApplicationInDraft,
		-- Get the count of Submitted
		COUNT(CASE WHEN apl.ApprovalStatus = 1 THEN 1 END) AS Submitted,
		-- Get the count of Developer Verified
		COUNT(CASE WHEN apl.ApprovalStatus = 3 THEN 1 END) AS DeveloperVerified,
		-- Get the count of Pag-ibig Verified
		COUNT(CASE WHEN apl.ApprovalStatus = 4  THEN 1 END) AS PagibigVerified,
		-- Get the count of Pagibig Deffered
		COUNT(CASE WHEN apl.ApprovalStatus IN (2) and x.RoleId = 3  THEN 1 END) AS  PagibigDeferred,
		-- Get the count of Developer Deffered
		COUNT(CASE WHEN apl.ApprovalStatus IN (2) and x.RoleId IN (5,2)   THEN 1 END) AS DeveloperDeferred,
		-- Get the count of Withdrawn
		COUNT(CASE WHEN apl.ApprovalStatus = 5 THEN 1 END) AS Withdrawn

	FROM ApplicantsPersonalInformation apl
	LEFT JOIN BeneficiaryInformation bi ON bi.UserId = apl.UserId
    LEFT JOIN ApprovalStatus aps ON aps.ReferenceId = apl.Id AND aps.ReferenceType = (select Id from Module where Code = 'APLCNTREQ')
	LEFT JOIN (
		SELECT ApprovalStatusId,ur.RoleId,[Status] FROM ApprovalLevel 
		LEFT JOIN [UserRole] ur ON ur.UserId = ApproverId  WHERE [Status] = 2) x 
		ON x.ApprovalStatusId = aps.ReferenceId
	WHERE apl.CompanyId = @companyId
	AND 1 = ( CASE WHEN @developerId IS NULL THEN 1
	WHEN @developerId IS NOT NULL AND @developerId = bi.PropertyDeveloperId THEN 1
	END)

RETURN 0
