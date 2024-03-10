CREATE PROCEDURE [dbo].[spPurposeOfLoan_Get]
@id INT
AS
	SELECT  * FROM PurposeOfLoan WHERE Id =@id
RETURN 0
