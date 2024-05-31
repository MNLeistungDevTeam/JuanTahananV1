CREATE PROCEDURE [dbo].[spBuyerConfirmationDocument_GetByDocumentId]
@documentId INT
AS
	SELECT bcd.*,
	       bc.Code BuyerConfirmationCode,
		   CASE WHEN bcd.[Status] = 3 THEN 'Approved'	
			    WHEN bcd.[Status] = 11 THEN 'Returned'
			END BuyerConfirmationStatus,
			bc.Email ApplicantEmail,
			CONCAT(u1.Firstname,' ',u1.MiddleName,' ',u1.LastName) ApplicantFullName,
			u1.FirstName ApplicantFirstName,
			ar.[Name] ApproverRole,
			CONCAT(u2.Firstname,' ',u2.MiddleName,' ',u2.LastName) ApproverFullName,
			u2.FirstName ApproverFirstName
	FROM BuyerConfirmationDocument bcd 
	LEFT JOIN (	SELECT aps1.*, aplvl.Remarks, aplvl.ApproverId, ur.RoleId ApproverRoleId
	FROM ApprovalStatus aps1
	LEFT JOIN (
				SELECT  
					aplvl1.* 
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
								 SELECT 
									TOP 1 Id 
								 FROM Module  WHERE Code = 'BCF-UPLOAD'
								)
	) aps ON bcd.Id = aps.ReferenceId
	LEFT JOIN [Role] ar ON aps.ApproverRoleId = ar.Id
LEFT JOIN [User] u2 ON aps.ApproverId = u2.Id
LEFT JOIN [User] u1 ON bcd.CreatedById = u1.Id
LEFT JOIN BuyerConfirmation bc ON bc.Code = bcd.ReferenceNo
WHERE bcd.ReferenceId = @documentId
RETURN 0
