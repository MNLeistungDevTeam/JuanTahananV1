CREATE PROCEDURE [dbo].[spBeneficiaryInformation_GetByPagibigNumber]
	@pagibigNumber NVARCHAR(255) 
AS
	SELECT bi.*,
			u.ProfilePicture
	from BeneficiaryInformation bi 
	LEFT JOIN [User] u ON u.Id = bi.UserId
	WHERE bi.PagibigNumber = @pagibigNumber
	
RETURN 0
