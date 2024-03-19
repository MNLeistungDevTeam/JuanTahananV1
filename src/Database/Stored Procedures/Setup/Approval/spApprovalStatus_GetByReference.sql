CREATE PROCEDURE [dbo].[spApprovalStatus_GetByReference]
@referenceId INT,
@referenceType INT,
@companyId INT
AS
	
     
	SELECT
		 * FROM vwTransactions vw
	WHERE vw.ReferenceId = @referenceId AND vw.ModuleId = @referenceType AND vw.CompanyId = @companyId
RETURN 0