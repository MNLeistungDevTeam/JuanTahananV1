CREATE PROCEDURE [dbo].[spDocument_GetApplicantDocumentsByDocumentType]
 @applicantCode NVARCHAR(255) ,
 @documentTypeId INT
AS
  SELECT d.* from Document d
  LEFT JOIN DocumentType dt ON dt.Id = d.DocumentTypeId 
  where ReferenceNo = @applicantCode AND d.DocumentTypeId = @documentTypeId;
RETURN 0
 