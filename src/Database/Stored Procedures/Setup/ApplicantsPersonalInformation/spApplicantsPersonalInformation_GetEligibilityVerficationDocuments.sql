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
 WHERE dt.Id IN (SELECT DocumentTypeId FROM  DocumentVerification WHERE [Type] = 1) --verification type documents
RETURN 0
