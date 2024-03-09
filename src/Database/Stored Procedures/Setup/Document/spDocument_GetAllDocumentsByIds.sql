CREATE PROCEDURE [dbo].[spDocument_GetAllDocumentsByIds]
    @applicationId INT,
	@documentTypeId INT
AS
 
    SELECT
        d.Id,
        d.[Name],
        dbo.FormatBytes(d.Size) AS FormattedSize,
        d.DateCreated,
        usrd.ApplicantsPersonalInformationId,
        usrd.DocumentTypeId
    FROM
        UserDocuments usrd
    LEFT JOIN
        Document d ON d.Id = usrd.DocumentId
	    WHERE usrd.DocumentTypeId = @documentTypeId
	    AND usrd.ApplicantsPersonalInformationId = @applicationId
RETURN 0
