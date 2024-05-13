CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetByUserId]
@userId INT
AS
	SELECT bc.*,
		CONCAT(u1.Firstname,' ',u1.MiddleName,' ',u1.LastName) ApplicantFullName,
		u1.Email ApplicantEmail,
		u1.FirstName ApplicantFirstName,
		ar.[Name] ApproverRole,
		CONCAT(u2.Firstname,' ',u2.MiddleName,' ',u2.LastName) ApproverFullName,
		u2.FirstName ApproverFirstName,
	CASE
			WHEN bc.ApprovalStatus = 0 THEN 'Application in Draft'
			WHEN bc.ApprovalStatus = 1 THEN 'Submitted'
			WHEN bc.ApprovalStatus = 3 THEN 'Developer Verified'
			WHEN bc.ApprovalStatus = 11 THEN 'For Resubmission'
		END ApplicationStatus,
		CONCAT(u2.LastName, ' ',u2.FirstName, ' ', u2.MiddleName) AS ApproverFullName,
		u2.Position AS ApproverRole,
		aps.Remarks,
		aps.ApproverId
	FROM BuyerConfirmation bc
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
	WHERE aps1.ReferenceType = (
    SELECT TOP 1 Id 
    FROM Module 
    WHERE Id = 8 OR Code = 'BCF-APLRQST'
)
	) aps ON bc.Id = aps.ReferenceId
	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
	LEFT JOIN [User] u2 ON aps.ApproverId = u2.Id
	LEFT JOIN [User] u1 ON bc.UserId = u1.Id
	WHERE bc.UserId = @userId  
 
RETURN 0
