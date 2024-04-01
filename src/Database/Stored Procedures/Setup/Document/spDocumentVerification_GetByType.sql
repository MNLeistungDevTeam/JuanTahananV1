CREATE PROCEDURE [dbo].[spDocumentVerification_GetByType]
	@type INT,
	@applicantCode NVARCHAR(255)
	 
AS
 SELECT 
    dv.*,
    dt.[Description],
    dt.Id AS DocumentTypeId,
    CONCAT(u1.FirstName, ' ', u1.LastName) AS CreatedBy,
    CONCAT(u2.FirstName, ' ', u2.LastName) AS ModifiedBy, 
    dt.[Description] AS DocumentTypeDescription,
    (SELECT COUNT(*)
    FROM Document d 
    WHERE d.DocumentTypeId = dv.DocumentTypeId 
    AND d.ReferenceNo = @applicantCode
    ) AS TotalDocumentCount
 FROM DocumentVerification dv
    LEFT JOIN DocumentType dt ON dt.Id = dv.DocumentTypeId
    LEFT JOIN [User] u1 ON u1.Id = dv.CreatedById
    LEFT JOIN [User] u2 ON u2.Id = dv.ModifiedById
 WHERE dv.[Type] = @type;
RETURN 0
