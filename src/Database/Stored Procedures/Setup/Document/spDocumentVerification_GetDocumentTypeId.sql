CREATE PROCEDURE [dbo].[spDocumentVerification_GetDocumentTypeId]
	@id int
AS
	SELECT
		dv.*
	FROM DocumentVerification dv
	LEFT JOIN DocumentType dt ON dv.DocumentTypeId = dt.Id
	WHERE 
		dv.DocumentTypeId = @id
RETURN 0
