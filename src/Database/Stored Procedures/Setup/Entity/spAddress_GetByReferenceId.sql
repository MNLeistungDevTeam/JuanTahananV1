CREATE PROCEDURE [dbo].[spAddress_GetByReferenceId]
	@referenceId int,
	@referenceType int
AS
BEGIN
	SELECT
		a.*, c.Code
	FROM
		[Address] a
		LEFT JOIN Country c ON a.CountryId = c.Id
	WHERE
		a.ReferenceId = @referenceId AND
		a.ReferenceType = @referenceType
END
