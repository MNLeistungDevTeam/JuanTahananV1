CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetTotalAppVerif]

AS
	SET NOCOUNT ON;

	SELECT
		-- Get the count of Submitted
		COUNT(CASE WHEN apl.ApprovalStatus = 6 THEN 1 END) AS Submitted,
		-- Get the count of Developer Verified
		COUNT(CASE WHEN apl.ApprovalStatus = 7 THEN 1 END) AS DeveloperVerified,
		-- Get the count of Pag-ibig Verified
		COUNT(CASE WHEN apl.ApprovalStatus = 8  THEN 1 END) AS PagibigVerified,
		-- Get the count of Pagibig Deffered
		COUNT(CASE WHEN apl.ApprovalStatus IN (9) and x.RoleId = 3   THEN 1 END) AS  PagibigDeferred,
		-- Get the count of Developer Deffered
		COUNT(CASE WHEN apl.ApprovalStatus IN (9) and x.RoleId IN (5,2)   THEN 1 END) AS DeveloperDeferred,
		-- Get the count of Withdrawn
		COUNT(CASE WHEN apl.ApprovalStatus = 10 THEN 1 END) AS Withdrawn

	FROM ApplicantsPersonalInformation apl
    LEFT JOIN ApprovalStatus aps ON aps.ReferenceId = apl.Id
	LEFT JOIN (
		SELECT ApprovalStatusId,ur.RoleId,[Status] FROM ApprovalLevel 
		LEFT JOIN [UserRole] ur ON ur.UserId = ApproverId  WHERE [Status] = 9) x 
		ON x.ApprovalStatusId = aps.ReferenceId	

RETURN 0
