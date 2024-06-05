CREATE PROCEDURE [dbo].[spUser_GetUsersByCompanyId]
 
	@companyId INT 
AS
	 SELECT * FROM (

    SELECT 
        u.*,
		CASE 
			WHEN 30 - datediff(minute, LockedTime, GETDATE()) > 0 THEN 'Locked'
			ELSE 'Unlocked'
		END LockStatus,
		30 - datediff(minute, LockedTime, GETDATE()) LockedDuration,
		r.[Name] UserRoleName,
		ur.RoleId as UserRoleId,
		c.[Name],
		bi.PropertyDeveloperId,
		CASE WHEN bi.PropertyDeveloperId IS NOT NULL THEN bi.PropertyDeveloperId
		WHEN bi.PropertyDeveloperId IS NULL THEN uc.CompanyId
		ELSE null
		END CompanyId

    FROM [User] u
    LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
    LEFT JOIN [Role] r ON ur.RoleId = r.Id
    LEFT JOIN UserCompany uc ON uc.UserId = u.Id
    LEFT JOIN Company c ON c.Id = uc.CompanyId
    LEFT JOIN BeneficiaryInformation bi ON bi.UserId = u.Id
 
 ) main


 WHERE
 1 = (CASE WHEN @companyId IS NULL  THEN 1
	WHEN @companyId IS NOT NULL AND  main.CompanyId = @companyId THEN 1 END)
 

 

RETURN 0
