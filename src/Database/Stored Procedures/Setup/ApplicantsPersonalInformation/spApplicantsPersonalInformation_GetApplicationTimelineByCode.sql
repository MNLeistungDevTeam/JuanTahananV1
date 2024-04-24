CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetApplicationTimelineByCode]
	@code NVARCHAR(144),
	@companyId INT
AS
	SET NOCOUNT ON;

	SELECT 
		apl.Code,
        0 ApprovalStatusNumber,
		'Application In Draft' ApplicationStatus,
		'Credit Verification' Stage,
		1 StageNo,
		0 ApproverRoleId,
		apl.DateCreated
		--1 AS NextApprovalStatus
	FROM
		ApplicantsPersonalInformation apl
	WHERE
		apl.Code = @code

	UNION ALL

	SELECT
        apl.Code,
        appLog.[Action] ApprovalStatusNumber,
        CASE
			WHEN appLog.[Action] = 0 THEN 'Application in Draft'
			WHEN appLog.[Action] = 1 THEN 'Application Submitted'
			WHEN appLog.[Action] = 3 THEN 'Verified by Developer'
			WHEN appLog.[Action] = 4 THEN 'Verified by Pag-IBIG'
			WHEN appLog.[Action] = 5 THEN 'Application Withdrawn'
			WHEN appLog.[Action] = 6 THEN 'Application Submitted'
			WHEN appLog.[Action] = 7 THEN 'Approved by Developer'
			WHEN appLog.[Action] = 8 THEN 'Approved by Pag-IBIG'
			WHEN appLog.[Action] = 10 THEN 'Application Withdrawn'
			WHEN appLog.[Action] = 11 THEN 'For Resubmission'
			ELSE 
				CASE
					WHEN ur2.RoleId = 3 THEN 'Deferred by Pag-IBIG'
					WHEN ur2.RoleId = 5 THEN 'Deferred by Developer'
				END 
		END ApplicationStatus,
		CASE
			WHEN appLog.[Action] IN (0,1,2,3,5,11) THEN 'Credit Verification'
			WHEN appLog.[Action] IN (4,6,7,9,10) THEN 'Application Completion'
			WHEN appLog.[Action] = 8 THEN 'Post-Approval'
		END Stage,
		CASE
			WHEN appLog.[Action] IN (0,1,2,3,5,11) THEN 1
			WHEN appLog.[Action]  IN(4,6,7,8,9,10) THEN 2
		END StageNo,
		ur2.RoleId ApproverRoleId,
        appLog.DateCreated
		--CASE WHEN LEAD(appLog.[Action]) OVER (ORDER BY appLog.DateCreated) IS NOT NULL THEN 1 ELSE 0 END AS NextApprovalStatus

    FROM ApplicantsPersonalInformation apl
    LEFT JOIN ApprovalLog appLog ON appLog.ReferenceId = apl.Id
	LEFT JOIN UserRole ur2 ON appLog.CreatedById = ur2.UserId
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
	LEFT JOIN (
					select Top 1 apl1.*  from ApprovalLog apl1
					LEFT JOIN [UserRole] ur ON ur.UserId = apl1.CreatedById  
					Where apl1.[Action] = 1  
					Order by apl1.DateCreated Desc
	) aplog ON   aplog.ReferenceId = apl.Id 

	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
    WHERE
        apl.Code = COALESCE(@code, apl.Code)
		AND appLog.ReferenceId IS NOT NULL -- No data will show if application status is on draft
		AND apl.CompanyId = @companyId
RETURN 0