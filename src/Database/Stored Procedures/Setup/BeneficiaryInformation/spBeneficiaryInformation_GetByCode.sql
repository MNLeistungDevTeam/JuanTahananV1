CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetByCode]
	@code NVARCHAR(100)
AS
	SELECT * FROM BeneficiaryInformation WHERE Code = @code
RETURN 0
