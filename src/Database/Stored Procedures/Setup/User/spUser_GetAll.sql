﻿CREATE PROCEDURE [dbo].[spUser_GetAll]
AS
BEGIN
	SELECT 
		u.*,
		CASE 
			WHEN 30 - datediff(minute, LockedTime, GETDATE()) > 0 THEN 'Locked'
			ELSE 'Unlocked'
		END LockStatus,
		30 - datediff(minute, LockedTime, GETDATE()) LockedDuration,
		r.[Name] UserRoleName,
		ur.RoleId as UserRoleId
	FROM [User] u
	LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
	LEFT JOIN [Role] r ON ur.RoleId = r.Id
END