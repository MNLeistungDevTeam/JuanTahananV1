CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetByPagibigNumber]
	@pagibigNumber NVARCHAR(255) 
AS
	SELECT * from BeneficiaryInformation WHERE PagibigNumber = @pagibigNumber
RETURN 0
