CREATE PROCEDURE [dbo].[spDocument_GetApplicantDocumentsByCode]
 @applicantCode NVARCHAR(255) 
AS
  SELECT * from Document where ReferenceNo = @applicantCode;
RETURN 0
