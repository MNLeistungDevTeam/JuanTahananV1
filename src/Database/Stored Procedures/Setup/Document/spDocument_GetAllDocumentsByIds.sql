CREATE PROCEDURE [dbo].[spDocument_GetAllDocumentsByIds]
    @ApplicationId INT,
	@DocumentTypeId INT
AS
BEGIN
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
	    WHERE usrd.DocumentTypeId = @DocumentTypeId
	    AND usrd.ApplicantsPersonalInformationId = @ApplicationId
END
