CREATE PROCEDURE [dbo].[spDocumentType_GetById]
	@id int
AS
	SELECT 
		dt.*
	FROM DocumentType dt
	WHERE dt.Id = @id
RETURN 0
