CREATE PROCEDURE [dbo].[spDocumentVerification_GetByType]
	@type INT
	 
AS
	SELECT dv.*,
	dt.[Description],
	CONCAT(u1.FirstName, ' ',u1.LastName) CreatedBy,
	CONCAT(u2.FirstName, ' ',u2.LastName) ModifiedBy, 
	dt.[Description] DocumentTypeDescription
	FROM DocumentVerification dv
	LEFT JOIN DocumentType dt ON dt.Id = dv.DocumentTypeId
	LEFT JOIN [User] u1 ON u1.Id = dv.CreatedById
	LEFT JOIN [User] u2 ON u2.Id = dv.ModifiedById
	WHERE dv.[Type] = @type
RETURN 0
