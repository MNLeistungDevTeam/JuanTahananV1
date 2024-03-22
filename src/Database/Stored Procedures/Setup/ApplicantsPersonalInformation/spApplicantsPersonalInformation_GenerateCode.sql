CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GenerateCode]
 
AS
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		TOP 1 MAX(Code) ApplicationCode
	FROM ApplicantsPersonalInformation
	WHERE MONTH(DateCreated) = MONTH(GETDATE()) 
		  AND YEAR(DateCreated) = YEAR(GETDATE())
RETURN 0
