CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetEligibilityVerficationDocuments]
	@applicantCode NVARCHAR(150)
AS



 
 SELECT 
 dt.Id DocumentTypeId,
 d.Id DocumentId,
 dt.[Description] DocumentTypeName,
 d.[Location] DocumentLocation,
 d.[Name] DocumentName,
 d.Size DocumentSize,
 d.FileType DocumentFileType

 FROM DocumentType dt
 LEFT JOIN Document d ON d.DocumentTypeId = dt.Id And d.ReferenceNo = @applicantCode
 WHERE dt.Id IN (5,3,4,6,7,8,9) 
RETURN 0
