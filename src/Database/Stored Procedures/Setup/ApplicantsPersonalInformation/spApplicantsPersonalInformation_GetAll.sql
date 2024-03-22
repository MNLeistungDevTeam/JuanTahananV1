CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetAll]
 
AS
	 SELECT 
		apl.*,
		CONCAT(u.LastName,', ',u.FirstName,'',u.MiddleName) ApplicantFullName,
		u.[Position] PositionName,  --applicant position
		0.00 As IncomeAmount,
		bi.PropertyDeveloperName Developer,
		bi.PropertyLocation ProjectLocation,
		'' Project,
		bi.PropertyUnitLevelName Unit,
		lpi.DesiredLoanAmount As LoanAmount,
		--CASE WHEN apl.ApprovalStatus = 1 Then 'Application in Draft'
		--	 WHEN  apl.ApprovalStatus = 2 Then 'Approved'
		--	 ELSE 'Defered'	
		--END ApplicationStatus
		CASE
			WHEN apl.ApprovalStatus = 0 THEN 'Application in Draft'
			WHEN apl.ApprovalStatus = 1 THEN 'Submitted'
			WHEN apl.ApprovalStatus = 3 THEN 'Developer Verified'
			WHEN apl.ApprovalStatus = 4 THEN 'PAG-IBIG Verified'
			WHEN apl.ApprovalStatus = 5 THEN 'Withdrawn'
			ELSE CONCAT('Deferred by ', ar.[Name])
		END ApplicationStatus
	FROM ApplicantsPersonalInformation apl
	LEFT JOIN BarrowersInformation bi ON bi.ApplicantsPersonalInformationId = apl.Id
	LEFT JOIN LoanParticularsInformation lpi ON lpi.ApplicantsPersonalInformationId = apl.Id
	LEFT JOIN [User] u on u.Id = apl.UserId
	LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
	LEFT JOIN [Role] r ON r.Id = ur.RoleId
	LEFT JOIN (
		SELECT aps1.*, aplvl.Remarks, aplvl.ApproverId,
		ur.RoleId ApproverRoleId
		FROM ApprovalStatus aps1
		LEFT JOIN (
			SELECT  aplvl1.* 
			FROM ApprovalLevel aplvl1
			INNER JOIN (
				SELECT ApprovalStatusId, MAX(Id) Id
				FROM ApprovalLevel
				GROUP BY ApprovalStatusId
			) x ON aplvl1.Id = x.Id
		) aplvl ON aps1.Id = aplvl.ApprovalStatusId
		LEFT JOIN [User] ua ON aplvl.ApproverId = ua.Id
		INNER JOIN UserRole ur ON ua.Id = ur.UserId
	) aps ON apl.Id = aps.ReferenceId
	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
RETURN 0