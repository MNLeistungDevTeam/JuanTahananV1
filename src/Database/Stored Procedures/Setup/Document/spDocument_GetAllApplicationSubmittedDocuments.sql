CREATE PROCEDURE [dbo].[spDocument_GetAllApplicationSubmittedDocuments]
    @applicationId INT
AS
	SELECT
		 ISNULL(dt.Id,0) AS DocumentTypeId,
		 dt.[Description],
		 ISNULL(COUNT(usrd.Id),0) AS TotalUploadedDocuments
		 FROM DocumentType dt
		 LEFT JOIN Document d ON d.DocumentTypeId =  dt.Id
		 LEFT JOIN UserDocuments usrd ON usrd.DocumentId = d.Id AND usrd.ApplicantsPersonalInformationId = @applicationId
		 GROUP BY
		 dt.Id,
		 dt.[Description]
RETURN 0
