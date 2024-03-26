CREATE PROCEDURE [dbo].[spDocument_GetApplicantDocumentsByCode]
 @applicantCode NVARCHAR(255) 
AS
    SELECT
	dc.*,
	dtype.[Description] AS DocuTypeDesc
  from Document dc 
  LEFT JOIN DocumentType dtype ON dtype.Id = dc.DocumentTypeId 
  where ReferenceNo = @applicantCode;

RETURN 0
