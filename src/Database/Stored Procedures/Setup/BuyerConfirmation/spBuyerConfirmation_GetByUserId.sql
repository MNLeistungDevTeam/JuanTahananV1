 CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetByUserId]
@userId INT
AS
SELECT 		
	bc.*,
		CONCAT(u1.Firstname,' ',u1.MiddleName,' ',u1.LastName) ApplicantFullName,
		u1.Email ApplicantEmail,
		u1.FirstName ApplicantFirstName,
		ar.[Name] ApproverRole,
		CONCAT(u2.Firstname,' ',u2.MiddleName,' ',u2.LastName) ApproverFullName,
		u2.FirstName ApproverFirstName,
  CASE
        WHEN bc.ApprovalStatus = 0 THEN 'Submitted'
        WHEN bc.ApprovalStatus = 3 AND bcd.[Status] = 1 THEN 'Sign and Submitted'
        WHEN bc.ApprovalStatus = 3 AND bcd.[Status] = 11 THEN 'For Resubmission' -- document resubmit
        WHEN bc.ApprovalStatus = 3 AND bcd.[Status] = 3 THEN 'Approved'
        WHEN bc.ApprovalStatus = 3 THEN 'Ready For Printing'
        WHEN bc.ApprovalStatus = 11 THEN 'For Revision'
    END AS ApplicationStatus,
		CONCAT(u2.LastName, ' ',u2.FirstName, ' ', u2.MiddleName) AS ApproverFullName,
		u2.Position AS ApproverRole,
		aps.Remarks,
		aps.ApproverId,
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	'' ELSE d.[Name]
	END [FileName],
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	'' ELSE d.[Location]
	END FileLocation,
    bcd.Id AS BuyerConfirmationDocumentId,
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	0 ELSE bcd.Id
	END BuyerConfirmationDocumentId,
    bcd.[Status] BuyerConfirmationDocumentStatus,
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	'' ELSE d.FileType
	END FileType,
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	'' ELSE d.Size
	END FileSize
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
	LEFT JOIN (
    SELECT 
        bcd1.*,
        ROW_NUMBER() OVER (PARTITION BY bcd1.ReferenceNo ORDER BY bcd1.DateCreated DESC) AS rn
    FROM BuyerConfirmationDocument bcd1
) bcd ON bcd.ReferenceNo = bc.Code AND bcd.rn = 1
LEFT JOIN Document d ON d.Id = bcd.ReferenceId
	WHERE bc.UserId = @userId
 
RETURN 0
