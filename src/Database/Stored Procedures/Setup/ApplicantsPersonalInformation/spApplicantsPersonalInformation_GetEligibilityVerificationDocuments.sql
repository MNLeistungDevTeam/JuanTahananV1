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
            CASE WHEN bi.OccupationStatus = 'Employed' THEN 1 
                 WHEN bi.OccupationStatus = 'Self-Employed' THEN 2 
                 WHEN bi.OccupationStatus = 'Others' THEN 3 
            END AS OccupationType,
            ROW_NUMBER() OVER (PARTITION BY dt.Id ORDER BY d.DateCreated DESC) AS DocumentSequence,
            COUNT(*) OVER (PARTITION BY dt.Id) AS DocumentCount
        FROM 
            DocumentType dt
        LEFT JOIN 
            Document d ON d.DocumentTypeId = dt.Id AND d.ReferenceNo = @applicantCode
        LEFT JOIN 
            ApplicantsPersonalInformation apl ON apl.Code = @applicantCode
        LEFT JOIN 
            BarrowersInformation bi ON bi.ApplicantsPersonalInformationId = apl.Id
        WHERE 
            dt.Id IN (SELECT DocumentTypeId FROM DocumentVerification WHERE [Type] = 1) --verification type documents
    )


    SELECT * FROM (
        SELECT 
            dd.DocumentTypeId,
            dd.DocumentParentId,
            dd.DocumentId,
            dd.DocumentTypeName,
            dd.DocumentLocation,
            dd.DocumentName,
            dd.DocumentSize,
            dd.DocumentFileType,
            dd.OccupationType,
            CASE 
                WHEN sd.[Type] IS NULL THEN dd.OccupationType
                ELSE sd.[Type]
            END AS [Type],
            CASE 
                WHEN dd.DocumentParentId IS NULL THEN '0' 
                ELSE '1' 
            END AS HasParentId,
            CASE 
                WHEN EXISTS (SELECT 1 FROM SubDocument sd WHERE sd.ParentId = dd.DocumentTypeId) THEN '1' 
                ELSE '0' 
            END AS HasSubdocument
        FROM 
            DocumentDetails dd
        LEFT JOIN 
            SubDocument sd ON sd.DocumentTypeId = dd.DocumentTypeId 
        WHERE 
            dd.DocumentSequence = 1 -- Select only the latest document per DocumentTypeId
    ) main
    WHERE 
        main.[Type] = main.OccupationType;


RETURN 0