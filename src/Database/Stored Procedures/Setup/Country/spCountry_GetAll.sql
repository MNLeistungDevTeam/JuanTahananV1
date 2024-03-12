CREATE PROCEDURE [dbo].[spCountry_GetAll]
AS
BEGIN
	SELECT 
		Country.*,
		CONCAT(RegUser.FirstName ,' ' ,RegUser.LastName) as CreatedByName,
		CONCAT(ModBy.FirstName,' ',ModBy.LastName) as ModifiedByName
	FROM Country
	LEFT JOIN [User] RegUser ON Country.CreatedById = RegUser.Id 
	LEFT JOIN [User] ModBy ON  Country.ModifiedById = ModBy.Id
END
