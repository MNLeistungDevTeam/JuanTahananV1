CREATE PROCEDURE [dbo].[spPropertyType_Get]
	@id INT
AS
	SELECT  * FROM PropertyType WHERE Id =@id
RETURN 0
