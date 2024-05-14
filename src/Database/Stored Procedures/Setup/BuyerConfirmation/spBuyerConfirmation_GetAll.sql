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
			WHEN bcf.ApprovalStatus = 3 THEN 'Ready For Printing'
			WHEN bcf.ApprovalStatus = 11 THEN 'For Resubmission'
		END ApplicationStatus,
		bcf.ApprovalStatus,
		bcf.DateCreated,
		bcf.DateModified
	FROM BuyerConfirmation bcf
RETURN 0
