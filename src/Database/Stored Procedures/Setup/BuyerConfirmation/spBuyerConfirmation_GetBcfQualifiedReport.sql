CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetBcfQualifiedReport]
AS
	SELECT 
		CONCAT(bcf.LastName, ', ', bcf.FirstName, ' ', bcf.MiddleName) AS FullName,
		CASE
			WHEN bcf.IsPagibigMember = 0 THEN 'Yes'
			ELSE 'No'
		END AS isPagibigMember,
		bcf.PagibigNumber AS PagibigNumber,
		CONVERT(VARCHAR, BirthDate, 101) AS BirthDate,
		bcf.MonthlySalary,
		CONCAT(bcf.PresentUnitName, ' ', PresentBuildingName, ' ', PresentLotName, ' ', PresentStreetName, ' ', PresentSubdivisionName, ' ',
				PresentBaranggayName, ' ', PresentMunicipalityName, ' ', PresentProvinceName, ' ', PresentZipCode) AS PresentHomeAddress,
		bcf.MobileNumber,
		bcf.Email,
		bcf.CompanyEmployerName,
		bcd.[Status]
	FROM BuyerConfirmation bcf
		LEFT JOIN (
					SELECT 
						bcd1.*,
						ROW_NUMBER() OVER (PARTITION BY bcd1.ReferenceNo ORDER BY bcd1.DateCreated DESC) AS rn
					FROM BuyerConfirmationDocument bcd1
				) bcd ON bcd.ReferenceNo = bcf.Code AND bcd.rn = 1
		LEFT JOIN Document d ON d.Id = bcd.ReferenceId
		LEFT JOIN PropertyLocation pl ON pl.Id = bcf.PropertyLocationId
		LEFT JOIN PropertyProject pp ON pp.Id = bcf.PropertyProjectId
		LEFT JOIN PropertyUnit pu ON pu.Id = bcf.ProjectUnitId
		LEFT JOIN Company dvl ON dvl.Id = bcf.PropertyDeveloperId
	WHERE bcd.[Status] = 3  -- Approved by Developer

RETURN 0
