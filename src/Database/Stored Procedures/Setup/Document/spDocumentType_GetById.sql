CREATE PROCEDURE [dbo].[spDocumentType_GetById]
	@id int
AS
	SELECT 
		dt.*,
		ft.[format] FileFormat
	FROM DocumentType dt
	LEFT JOIN FileType ft ON ft.Id = dt.FileType
	WHERE dt.Id = @id
RETURN 0
