CREATE PROCEDURE [dbo].[spDocument_GetApplicantDocumentsByDocumentType]
 @applicantCode NVARCHAR(255) ,
 @documentTypeId INT
AS
   SELECT TOP 1 d.*
    FROM Document d
    LEFT JOIN DocumentType dt ON dt.Id = d.DocumentTypeId 
    WHERE ReferenceNo = @applicantCode AND d.DocumentTypeId = @documentTypeId
    ORDER BY d.DateCreated DESC;
RETURN 0
 