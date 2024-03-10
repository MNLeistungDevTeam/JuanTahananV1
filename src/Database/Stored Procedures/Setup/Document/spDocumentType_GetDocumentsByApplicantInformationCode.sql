CREATE PROCEDURE [dbo].[spDocumentType_GetDocumentsByApplicantInformationCode]
	 @applicantCode NVARCHAR(255) 
AS

 

 SELECT
        dt.Id,
       dt.[Description],
       COUNT(CASE WHEN d.ReferenceNo = @applicantCode THEN 1 ELSE NULL END) AS TotalDocumentCount
FROM DocumentType dt
LEFT JOIN Document d ON d.DocumentTypeId = dt.Id
GROUP BY dt.Id, dt.[Description];


RETURN 0
