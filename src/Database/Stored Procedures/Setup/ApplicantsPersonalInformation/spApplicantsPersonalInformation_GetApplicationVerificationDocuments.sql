CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetApplicationVerificationDocuments]
	@applicantCode NVARCHAR(150)
AS
 --SELECT 
 --dt.Id DocumentTypeId,
 --d.Id DocumentId,
 --dt.[Description] DocumentTypeName,
 --d.[Location] DocumentLocation,
 --d.[Name] DocumentName,
 --d.Size DocumentSize,
 --d.FileType DocumentFileType
 --FROM DocumentType dt
 --LEFT JOIN Document d ON d.DocumentTypeId = dt.Id And d.ReferenceNo = @applicantCode
 --WHERE dt.Id IN (SELECT DocumentTypeId FROM  DocumentVerification WHERE [Type] = 2) --verification type documents

 
WITH DocumentDetails AS (
    SELECT 
        dt.Id AS DocumentTypeId,
        d.Id AS DocumentId,
        dt.[Description] AS DocumentTypeName,
        d.[Location] AS DocumentLocation,
        d.[Name] AS DocumentName,
        d.Size AS DocumentSize,
        d.FileType AS DocumentFileType,
        ROW_NUMBER() OVER (PARTITION BY dt.Id ORDER BY d.DateCreated) AS DocumentSequence,
        COUNT(*) OVER (PARTITION BY dt.Id) AS DocumentCount
    FROM 
        DocumentType dt
    LEFT JOIN 
        Document d ON d.DocumentTypeId = dt.Id AND d.ReferenceNo = @applicantCode
    WHERE 
        dt.Id IN (SELECT DocumentTypeId FROM DocumentVerification WHERE [Type] = 2) --Loan Application type documents
)

SELECT 
    DocumentTypeId,
    DocumentId,
    DocumentTypeName,
    DocumentLocation,
    DocumentName,
    DocumentSize,
    DocumentFileType,
    CASE WHEN DocumentId IS NULL THEN NULL ELSE ROW_NUMBER() OVER (PARTITION BY DocumentTypeId ORDER BY DocumentId) END AS DocumentSequence
FROM 
    DocumentDetails;



RETURN 0

RETURN 0
