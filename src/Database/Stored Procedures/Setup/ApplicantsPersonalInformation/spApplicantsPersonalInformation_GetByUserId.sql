CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetByUserId]
@userId INT
AS
SELECT TOP 1 
		apl.*,
		CONCAT(u1.Firstname,' ',u1.MiddleName,' ',u1.LastName) ApplicantFullName,
		u1.Email ApplicantEmail,
		u1.FirstName ApplicantFirstName,
		ar.[Name] ApproverRole,
		CONCAT(u2.Firstname,' ',u2.MiddleName,' ',u2.LastName) ApproverFullName,
		u2.FirstName ApproverFirstName,
		CASE
			WHEN apl.ApprovalStatus = 0 THEN 'Application in Draft'
			WHEN apl.ApprovalStatus = 1 THEN 'Submitted'
			WHEN apl.ApprovalStatus = 3 THEN 'Developer Verified'
			WHEN apl.ApprovalStatus = 4 THEN 'PAG-IBIG Verified'
			WHEN apl.ApprovalStatus = 5 THEN 'Withdrawn'
			WHEN apl.ApprovalStatus = 6 THEN 'Submitted'
			WHEN apl.ApprovalStatus = 7 THEN 'Developer Approved'
			WHEN apl.ApprovalStatus = 8 THEN 'PAG-IBIG Approved'
			WHEN apl.ApprovalStatus = 9 THEN 'Withdrawn'
			ELSE CONCAT('Deferred by ', ar.[Name])
		END ApplicationStatus,
			CASE
			WHEN apl.ApprovalStatus IN (0,1,2,3,4,5) THEN 'For Verification Approval'
			WHEN apl.ApprovalStatus  IN(6,7,8,9) THEN 'For Application Approval'
		END Stage,
		CASE
			WHEN apl.ApprovalStatus IN (0,1,2,3,4,5) THEN 1
			WHEN apl.ApprovalStatus  IN(6,7,8,9) THEN 2
		END StageNo,
		aps.Remarks	
	FROM ApplicantsPersonalInformation apl
	LEFT JOIN (	SELECT aps1.*, aplvl.Remarks, aplvl.ApproverId,
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
	LEFT JOIN [User] u2 ON aps.ApproverId = u2.Id
	LEFT JOIN [User] u1 ON apl.UserId = u1.Id
	WHERE apl.UserId = @userId
--	ORDER BY DateCreated DESC
RETURN 0
