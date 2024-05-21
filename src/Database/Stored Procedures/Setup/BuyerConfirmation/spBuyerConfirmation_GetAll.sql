 CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetAll]
AS
	 

	  SELECT 
    bcf.Id,
    bcf.Code,
    CONCAT(bcf.LastName, ', ', bcf.FirstName, ' ', bcf.MiddleName) AS ApplicantFullName,
    bcf.PagibigNumber,
    bcf.MonthlySalary,
    bcf.ProjectProponentName,
    bcf.HouseUnitModel,
    CASE
        WHEN bcf.ApprovalStatus = 0 THEN 'Submitted'
        WHEN bcf.ApprovalStatus = 3 AND bcd.[Status] = 1 THEN 'Sign and Submitted'
        WHEN bcf.ApprovalStatus = 3 AND bcd.[Status] = 11 THEN 'For Resubmission' -- document resubmit
        WHEN bcf.ApprovalStatus = 3 AND bcd.[Status] = 3 THEN 'Approved'
        WHEN bcf.ApprovalStatus = 3 THEN 'Ready For Printing'
        WHEN bcf.ApprovalStatus = 11 THEN 'For Revision'
    END AS ApplicationStatus,
    bcf.ApprovalStatus,
    bcf.DateCreated,
    bcf.DateModified,
	CASE WHEN bcd.[Status] = 11   Then -- Returned
	'' ELSE d.[Name]
	END [FileName],
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	'' ELSE d.[Location]
	END FileLocation,
    bcd.Id AS BuyerConfirmationDocumentId,
	CASE WHEN bcd.[Status] = 11 Then -- Returned
	0 ELSE bcd.Id
	END BuyerConfirmationDocumentId,
    bcd.[Status] BuyerConfirmationDocumentStatus
FROM BuyerConfirmation bcf
LEFT JOIN (
    SELECT 
        bcd1.*,
        ROW_NUMBER() OVER (PARTITION BY bcd1.ReferenceNo ORDER BY bcd1.DateCreated DESC) AS rn
    FROM BuyerConfirmationDocument bcd1
) bcd ON bcd.ReferenceNo = bcf.Code AND bcd.rn = 1
LEFT JOIN Document d ON d.Id = bcd.ReferenceId
ORDER BY 
    CASE 
        WHEN bcf.ApprovalStatus = 0 THEN 0 -- Rows with ApprovalStatus = 0 come first
        ELSE 1 -- All other rows
    END,
    -- Add additional sorting criteria as needed
    bcd.DateModified DESC; -- For example, sorting by DateCreated in descending order

RETURN 0

 
 