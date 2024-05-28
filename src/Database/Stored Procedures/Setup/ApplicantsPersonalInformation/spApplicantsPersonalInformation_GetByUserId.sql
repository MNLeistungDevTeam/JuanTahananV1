﻿CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetByUserId]
	@userId INT,
	@companyId INT
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
			WHEN apl.ApprovalStatus = 4 THEN 'Pag-IBIG Verified'
			WHEN apl.ApprovalStatus = 5 THEN 'Withdrawn'
			WHEN apl.ApprovalStatus = 6 THEN 'Submitted'
			WHEN apl.ApprovalStatus = 7 THEN 'Developer Approved'
			WHEN apl.ApprovalStatus = 8 THEN 'Pag-IBIG Approved'
			WHEN apl.ApprovalStatus = 10 THEN 'Withdrawn'
			WHEN apl.ApprovalStatus = 11 THEN 'For Resubmission'
			ELSE CONCAT('Deferred by ', ar.[Name])
		END ApplicationStatus,
		CASE
			WHEN apl.ApprovalStatus IN (0,1,2,3,5,11) THEN 'Credit Verification'
			WHEN apl.ApprovalStatus IN (4,6,7,9,10) THEN 'Application Completion'
			WHEN apl.ApprovalStatus = 8 THEN 'Post-Approval'
		END Stage,
		CASE
			WHEN apl.ApprovalStatus IN (0,1,2,3,5,11) THEN 1
			WHEN apl.ApprovalStatus  IN(4,6,7,8,9,10) THEN 2
		END StageNo,
		CONCAT(u2.LastName, ' ',u2.FirstName, ' ', u2.MiddleName) AS ApproverFullName,
		u2.Position AS ApproverRole,
		aps.Remarks,
		aps.ApproverId,
		bi.PropertyProjectId,
		bi.PropertyUnitId,
		bi.PropertyDeveloperId,
		bi.PropertyLocationId,
		pp.[Name] PropertyProjectName,
		pl.[Name] PropertyLocationName,
		pu.[Description] PropertyUnitDescription,
		cl.[Location] PropertyDeveloperLogo,
		pp.ProfileImage PropertyProjectLogo,
		pu.ProfileImage PropertyUnitLogo
	FROM ApplicantsPersonalInformation apl
	LEFT JOIN BeneficiaryInformation bi ON bi.UserId = apl.UserId
	LEFT JOIN PropertyLocation pl ON pl.Id = bi.PropertyLocationId
	LEFT JOIN PropertyProject pp ON pp.Id = bi.PropertyProjectId
	LEFT JOIN PropertyUnit pu ON pu.Id = bi.PropertyUnitId
	LEFT JOIN Company c ON c.Id = bi.PropertyDeveloperId
	LEFT JOIN CompanyLogo cl ON cl.CompanyId = c.Id 
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
		WHERE aps1.ReferenceType =   (SELECT Id from Module WHERE Id = 8 OR Code = 'APLCNTREQ')
	) aps ON apl.Id = aps.ReferenceId
	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
	LEFT JOIN [User] u2 ON aps.ApproverId = u2.Id
	LEFT JOIN [User] u1 ON apl.UserId = u1.Id
	WHERE apl.UserId = @userId AND apl.CompanyId = @companyId
	ORDER BY DateCreated DESC
RETURN 0
