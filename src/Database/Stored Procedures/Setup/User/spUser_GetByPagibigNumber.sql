CREATE PROCEDURE [dbo].[spUser_GetByPagibigNumber]
	@pagibigNumber NVARCHAR(255)
	 
AS
	SELECT * FROM [User] WHERE PagibigNumber = @pagibigNumber
RETURN 0
