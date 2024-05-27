 CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetAll]
	@roleId INT,
	@companyId INT
AS
  
 SELECT * FROM (
  
 SELECT main.*,aplog2.DateCreated DateSubmitted ,  ROW_NUMBER() OVER (PARTITION BY aps2.ReferenceId ORDER BY aplog2.DateCreated ASC) AS rn
FROM (

	 SELECT 
		apl.*,
		CONCAT(u.LastName,', ',u.FirstName,' ',u.MiddleName) ApplicantFullName,
		u.[Position] PositionName,  --applicant position
		0.00 As IncomeAmount,
		c.[Name] Developer,     ---remove this
		pl.[Name] ProjectLocation,   ---remove this
		pp.[Name] Project,   ---remove this
		pu.[Description] Unit,   ---remove this
		c.[Name] PropertyDeveloperName,
		pl.[Name] PropertyLocationName,
		pp.[Name] PropertyProjectName,
		pu.[Description] PropertyUnitDescription,
		cl.[Location] PropertyDeveloperLogo,
		pp.ProfileImage PropertyProjectLogo,
		pu.ProfileImage PropertyUnitLogo,

		lpi.DesiredLoanAmount As LoanAmount,
		--CASE WHEN apl.ApprovalStatus = 1 Then 'Application in Draft'
		--	 WHEN  apl.ApprovalStatus = 2 Then 'Approved'
		--	 ELSE 'Defered'	
		--END ApplicationStatus
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
		apl.ApprovalStatus ApprovalStatusNumber,
		CASE
			WHEN apl.ApprovalStatus = 0 THEN NULL
			ELSE 	apl.DateModified 
		END LastUpdated,
		CONCAT(u2.LastName, ' ',u2.FirstName, ' ', u2.MiddleName) AS ApproverFullName,
		u2.Position AS ApproverRole,
		aps.Remarks  
	FROM ApplicantsPersonalInformation apl
	LEFT JOIN BarrowersInformation bi ON bi.ApplicantsPersonalInformationId = apl.Id
	LEFT JOIN BeneficiaryInformation bfi ON bfi.UserId = apl.UserId
	LEFT JOIN LoanParticularsInformation lpi ON lpi.ApplicantsPersonalInformationId = apl.Id
	LEFT JOIN [User] u on u.Id = apl.UserId
	LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
	LEFT JOIN [Role] r ON r.Id = ur.RoleId
	LEFT JOIN Company c ON c.Id = bfi.PropertyDeveloperId
	LEFT JOIN CompanyLogo cl ON cl.CompanyId = c.Id 
	LEFT JOIN PropertyProject pp ON pp.Id = bfi.PropertyProjectId
	LEFT JOIN PropertyUnit pu ON pu.Id = bfi.PropertyUnitId
	LEFT JOIN PropertyLocation pl ON pl.Id = bfi.PropertyLocationId
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
		WHERE aps1.ReferenceType =  (SELECT Id from Module WHERE Id = 8 OR Code = 'APLCNTREQ')
	) aps ON apl.Id = aps.ReferenceId 
	

	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
	LEFT JOIN [User] u2 ON u2.Id = aps.ApproverId

	 WHERE 
    1 = (
        CASE  
            WHEN @roleId = 1 THEN 1 --Admin
			WHEN @roleId = 2 THEN  --LGU
			CASE WHEN apl.ApprovalStatus IN  (1,2,3,4,5,6,7,8,9,10,11) THEN 1 ELSE 0 END
            WHEN @roleId = 4 THEN --Beneficiary
                CASE WHEN apl.ApprovalStatus IN (0,1,2,3,4,5,6,7,8,9,10,11) THEN 1 ELSE 0 END
		      WHEN @roleId = 5 THEN --Developer 
                CASE WHEN apl.ApprovalStatus IN (1,2,3,4,5,6,7,8,9,10,11) THEN 1 ELSE 0 END
				WHEN @roleId = 3 THEN --Pagibig 
                CASE WHEN apl.ApprovalStatus IN (3,4,5,6,7,8,9,10,11) THEN 1 ELSE 0 END
        END
    ) 
	AND apl.CompanyId = @companyId 


	--ORDER BY apl.DateModified DESC;
	--ORDER BY apl.Id DESC, apl.DateModified DESC;
	
	) MAIN
	LEFT JOIN ApprovalStatus aps2 ON main.Id = aps2.ReferenceId  and aps2.ReferenceType = (SELECT Id from Module WHERE Id = 8 OR Code = 'APLCNTREQ')


	LEFT JOIN ApprovalLog aplog2 ON aplog2.ReferenceId = aps2.ReferenceId and aplog2.[Action] = 1 and aplog2.CreatedById = aps2.UserId
	) MAIN2
	WHERE MAIN2.rn = 1 

 ORDER BY 
    CASE 
        WHEN ApprovalStatus = 0 THEN 0 -- Rows with ApprovalStatus = 0 come first
        ELSE 1 -- All other rows
    END,
    -- Add additional sorting criteria as needed
    MAIN2.LastUpdated DESC; -- For example, sorting by DateCreated in descending order


RETURN 0