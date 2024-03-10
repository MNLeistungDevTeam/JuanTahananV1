CREATE PROCEDURE [dbo].[spCompany_Get]
	@id INT
AS
BEGIN
	SELECT
		c.*,
		CONCAT(a.StreetAddress1, ' ', a.StreetAddress2, ' ', a.Baranggay, ' ', a.CityMunicipality, ' ', a.StateProvince, ' ', a.Region, ' ', co.[Name], ' ', a.PostalCode) [Address],
		EOMONTH(DATEADD(MONTH, -1, GETDATE())) MinDate,
		DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), ISNULL(c.PostingPeriod, DAY(EOMONTH(GETDATE())))) MaxDate	 
	FROM Company c
	LEFT JOIN [Address] a ON a.ReferenceId = c.Id AND a.IsDefault = 1 AND a.ReferenceType = 1
	LEFT JOIN Country co ON a.CountryId = co.Id
	WHERE c.Id = @id;
END