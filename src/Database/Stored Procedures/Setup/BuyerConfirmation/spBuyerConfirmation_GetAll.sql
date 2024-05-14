 CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetAll]
AS
	 

	 	SELECT 
		bcf.Code,
		CONCAT(bcf.LastName, ', ',bcf.FirstName, ' ',bcf.MiddleName) ApplicantFullName,
		bcf.PagibigNumber,
		bcf.MonthlySalary,
		bcf.ProjectProponentName,
		bcf.HouseUnitModel,
		CASE
			WHEN bcf.ApprovalStatus = 0 THEN 'Application in Draft'
			WHEN bcf.ApprovalStatus = 3 AND bcd.[Status] = 1  THEN 'Sign and Submitted'
			WHEN bcf.ApprovalStatus = 3 AND bcd.[Status] = 3  THEN 'Approved'
			WHEN bcf.ApprovalStatus = 3 THEN 'Ready For Printing'
			WHEN bcf.ApprovalStatus = 11 THEN 'For Resubmission'
		END ApplicationStatus,
		bcf.ApprovalStatus,
		bcf.DateCreated,
		bcf.DateModified,
		d.[Name] [FileName],
		d.[Location] FileLocation,
		bcf.Id  BuyerConfirmationDocumentId
	FROM BuyerConfirmation bcf
	LEFT JOIN Document d ON d.ReferenceNo = bcf.Code  And DocumentTypeId = 0
	LEFT JOIN BuyerConfirmationDocument bcd ON bcd.ReferenceId = d.Id and bcd.[Status] != 11
RETURN 0
