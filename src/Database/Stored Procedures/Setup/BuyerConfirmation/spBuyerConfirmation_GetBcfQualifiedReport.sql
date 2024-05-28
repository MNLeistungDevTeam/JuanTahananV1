 CREATE PROCEDURE [dbo].[spBuyerConfirmation_GetBcfQualifiedReport]
 
 @locationId INT,
 @projectId INT,
 @developerId INT
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
		bcd.[Status],
		bcf.PropertyLocationId,
		bcf.PropertyDeveloperId,
		bcf.PropertyProjectId,
		bcf.ProjectUnitId,
		pp.[Name] PropertyProjectName,
		pl.[Name] PropertyLocationName,
		c.[Name] PropertyDeveloperName,
		bcd.DateModified LastUpdate

	FROM BuyerConfirmation bcf
		LEFT JOIN (
					SELECT 
						bcd1.*,
						ROW_NUMBER() OVER (PARTITION BY bcd1.ReferenceNo ORDER BY bcd1.DateCreated DESC) AS rn
					FROM BuyerConfirmationDocument bcd1
				) bcd ON bcd.ReferenceNo = bcf.Code AND bcd.rn = 1              --get the top 1 latest document per bcf
		LEFT JOIN Document d ON d.Id = bcd.ReferenceId
		LEFT JOIN PropertyLocation pl ON pl.Id = bcf.PropertyLocationId
		LEFT JOIN PropertyProject pp ON pp.Id = bcf.PropertyProjectId
		LEFT JOIN PropertyUnit pu ON pu.Id = bcf.ProjectUnitId
		LEFT JOIN Company c ON c.Id = bcf.PropertyDeveloperId
	WHERE bcd.[Status] = 3  -- Approved by Developer
	AND 1 = 
	(CASE WHEN @locationId IS NULL THEN 1
		  WHEN @locationId IS NOT NULL AND @locationId = bcf.PropertyLocationId THEN 1
	END)
	AND 1 = 
	(CASE WHEN @projectId IS NULL THEN 1
		  WHEN @projectId IS NOT NULL AND @projectId = bcf.PropertyProjectId THEN 1
	END)
	AND 1 = 
	(CASE WHEN @developerId IS NULL THEN 1
		  WHEN @developerId IS NOT NULL AND @developerId = bcf.PropertyDeveloperId THEN 1
	END)
RETURN 0
 