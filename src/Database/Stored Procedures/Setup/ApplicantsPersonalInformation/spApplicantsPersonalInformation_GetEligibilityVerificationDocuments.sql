CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetEligibilityVerificationDocuments]
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
 --WHERE dt.Id IN (SELECT DocumentTypeId FROM  DocumentVerification WHERE [Type] = 1) --verification type documents

 
WITH DocumentDetails AS (
    SELECT 
        dt.Id AS DocumentTypeId,
        d.Id AS DocumentId,
        dt.[Description] AS DocumentTypeName,
        d.[Location] AS DocumentLocation,
        d.[Name] AS DocumentName,
        d.Size AS DocumentSize,
        d.FileType AS DocumentFileType,
        dt.ParentId AS DocumentParentId,
        ROW_NUMBER() OVER (PARTITION BY dt.Id ORDER BY d.DateCreated) AS DocumentSequence,
        COUNT(*) OVER (PARTITION BY dt.Id) AS DocumentCount
    FROM 
        DocumentType dt
    LEFT JOIN 
        Document d ON d.DocumentTypeId = dt.Id AND d.ReferenceNo = @applicantCode
    WHERE 
        dt.Id IN (SELECT DocumentTypeId FROM DocumentVerification WHERE [Type] = 1) --verification type documents
)

SELECT 
    dd.DocumentTypeId,
    dd.DocumentParentId,
    dd.DocumentId,
    dd.DocumentTypeName,
    dd.DocumentLocation,
    dd.DocumentName,
    dd.DocumentSize,
    dd.DocumentFileType,
    CASE WHEN dd.DocumentId IS NULL THEN NULL ELSE ROW_NUMBER() OVER (PARTITION BY dd.DocumentTypeId ORDER BY dd.DocumentId) END AS DocumentSequence,
    CASE 
        WHEN dd.DocumentParentId IS NOT NULL THEN 'Yes' 
        ELSE CASE WHEN EXISTS (SELECT 1 FROM SubDocument sd WHERE sd.DocumentTypeId = dd.DocumentTypeId) THEN 'Yes' ELSE 'No' END
    END AS HasParentId,
    CASE WHEN EXISTS (SELECT 1 FROM SubDocument sd WHERE sd.ParentId = dd.DocumentTypeId) THEN 'Yes' ELSE 'No' END AS HasSubdocument
FROM 
    DocumentDetails dd;



RETURN 0
