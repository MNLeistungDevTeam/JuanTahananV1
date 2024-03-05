CREATE PROCEDURE [dbo].[spDocumentType_GetAllUserDocumentTypes]
AS
  BEGIN
    SELECT
        dct.Id,
        dct.[Description],
        COUNT(DISTINCT dc.Id) AS TotalDocumentCount,
        usrc.UserName AS CreatedBy,
        dct.DateCreated,
        usrm.UserName AS ModifiedBy,
        dct.DateModified
    FROM 
        DocumentType dct
    LEFT JOIN 
        [User] usrc ON usrc.Id = dct.CreatedById
    LEFT JOIN 
        [User] usrm ON usrm.Id = dct.ModifiedById
    LEFT JOIN 
        Document dc ON dc.DocumentTypeId = dct.Id
    WHERE
         dct.DateDeleted IS NULL
    GROUP BY
        dct.Id,
        dct.[Description],
        usrc.UserName,
        dct.DateCreated,
        usrm.UserName,
        dct.DateModified;
END
