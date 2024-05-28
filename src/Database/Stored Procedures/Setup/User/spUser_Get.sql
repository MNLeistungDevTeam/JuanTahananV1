CREATE PROCEDURE [dbo].[spUser_Get]
	@id INT
AS
BEGIN
	SELECT 
		u.*,
		CASE 
			WHEN 30 - datediff(minute, LockedTime, GETDATE()) > 0 THEN 'Locked'
			ELSE 'Unlocked'
		END LockStatus,
		30 - datediff(minute, LockedTime, GETDATE()) LockedDuration,
		r.[Description] UserRoleName,
		r.[Name] UserRoleCode,
		r.Id as UserRoleId,
		uc.CompanyId DeveloperId
	FROM [User] u
	LEFT JOIN [UserRole] ur on ur.UserId = u.Id
	LEFT JOIN [Role] r ON ur.RoleId = r.Id
	LEFT JOIN UserCompany uc ON uc.UserId = u.Id
	WHERE u.Id = @id
END