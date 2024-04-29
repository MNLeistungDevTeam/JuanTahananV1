CREATE PROCEDURE [dbo].[spDocumentVerification_GetByType]
	@type INT,
	@applicantCode NVARCHAR(255)
	 
AS
 SELECT main.* FROM (
    SELECT 
        dv.*,
        dt.[Description],
        CONCAT(u1.FirstName, ' ', u1.LastName) AS CreatedBy,
        CONCAT(u2.FirstName, ' ', u2.LastName) AS ModifiedBy, 
        CASE 
            WHEN dt.FileType = 1 THEN 'Pdf' 
            WHEN dt.FileType = 2 THEN 'Docx' 
            WHEN dt.FileType = 3 THEN 'Txt' 
            WHEN dt.FileType = 4 THEN 'Xlsx' 
            WHEN dt.FileType = 5 THEN 'Images*' 
            ELSE ''
        END DocumentFileType,
        dt.[Description] AS DocumentTypeDescription,
        (SELECT COUNT(*)
         FROM Document d 
         WHERE d.DocumentTypeId = dv.DocumentTypeId 
         AND d.ReferenceNo = @applicantCode
        ) AS TotalDocumentCount,
        CASE 
            WHEN  sd.[Type] is null AND bi.OccupationStatus = 'Employed' THEN 1
            WHEN  sd.[Type] is null AND bi.OccupationStatus = 'Self-Employed' THEN 2
            WHEN  sd.[Type] is null AND bi.OccupationStatus = 'Others' THEN 3
            ELSE sd.[Type]
        END AS SubDocumentType, -- Renamed alias to avoid conflict
        CASE 
            WHEN bi.OccupationStatus = 'Employed' THEN 1 
            WHEN bi.OccupationStatus = 'Self-Employed' THEN 2 
            WHEN bi.OccupationStatus = 'Others' THEN 3 
        END AS OccupationType -- Renamed alias to avoid conflict
    FROM DocumentVerification dv
    LEFT JOIN DocumentType dt ON dt.Id = dv.DocumentTypeId
    LEFT JOIN SubDocument sd ON sd.DocumentTypeId = dt.Id
    LEFT JOIN [User] u1 ON u1.Id = dv.CreatedById
    LEFT JOIN [User] u2 ON u2.Id = dv.ModifiedById
    LEFT JOIN ApplicantsPersonalInformation api ON api.Code = @applicantCode
    LEFT JOIN BarrowersInformation bi ON bi.ApplicantsPersonalInformationId = api.Id
    WHERE dv.[Type] = @type
) main
WHERE main.SubDocumentType = main.OccupationType;
RETURN 0
