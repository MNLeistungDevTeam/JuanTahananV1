CREATE PROCEDURE [dbo].[spDocumentType_GetInquiry]

AS
 SELECT dt.*,
 CASE WHEN dv.[Type] = 1   THEN  'Eligibility Verification Attachment'
 WHEN dv.[Type] = 2 THEN 'Application Attachment'
 ELSE ''
 END VerificationTypeDescription,
 dv.[Type] VerificationType,
 dv.Id DocumentVerificationId,
 CONCAT(u1.FirstName,' ',u1.LastName) CreatedBy,
 CONCAT(u2.FirstName,' ',u2.LastName) ModifiedBy
 FROM DocumentType dt
LEFT JOIN DocumentVerification dv ON dv.DocumentTypeId = dt.Id
LEFT JOIN [User] u1 ON u1.Id = dt.CreatedById
LEFT JOIN [User] u2 ON u2.Id = dt.ModifiedById
RETURN 0
