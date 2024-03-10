CREATE PROCEDURE [dbo].[spRoleAccess_GetRoleByModuleCode]
	@userId int,
	@moduleCode NVARCHAR(50)
AS

	IF @userId = 0 SET @userId = NULL
	
	SELECT
		m.Code,
		u.Id UserId,
		r.[Name] RoleName,
		ra.*,
		CAST (CASE
			WHEN ra.CanRead = 1 OR ra.CanModify = 1 OR ra.CanDelete = 1 OR ra.CanCreate = 1 THEN 1
			ELSE 0
		END as bit) HasAccess
	FROM
		[User] u

		INNER JOIN UserRole ur ON ur.UserId = u.Id
		INNER JOIN [Role] r ON ur.RoleId = r.Id
		INNER JOIN RoleAccess ra ON r.Id = ra.RoleId
		INNER JOIN Module m ON ra.ModuleId = m.Id
	WHERE
		u.Id = COALESCE(@userId, u.Id)
		AND m.Code = @moduleCode

RETURN 0
