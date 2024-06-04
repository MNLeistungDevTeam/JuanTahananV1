CREATE PROCEDURE [dbo].[spUser_GetUsersByCompanyId]
	@userId INT,
	@companyId INT = 0
AS
	SELECT 
        @companyId = uc.CompanyId
    FROM [User] u
    LEFT JOIN UserCompany uc ON uc.UserId = u.Id
    WHERE u.Id = @userId;

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
		bi.PropertyDeveloperId
    FROM [User] u
    LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
    LEFT JOIN [Role] r ON ur.RoleId = r.Id
    LEFT JOIN UserCompany uc ON uc.UserId = u.Id
    LEFT JOIN Company c ON c.Id = uc.CompanyId
    LEFT JOIN BeneficiaryInformation bi ON bi.UserId = u.Id AND c.Id = bi.PropertyDeveloperId
    WHERE @userId = 1 OR (RoleId IN (5, 1, 4) AND uc.CompanyId = @companyId);

RETURN 0
