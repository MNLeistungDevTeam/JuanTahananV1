CREATE PROCEDURE [dbo].[spRole_GetAllRoles]
AS
  BEGIN
    SELECT
        rl.Id,
        rl.[Name],
        rl.[Description],
        rl.DateCreated,
        rl.DateModified,
        rl.IsDisabled,
        rl.IsLocked,
        COUNT(DISTINCT rla.Id) AS TotalModulesCount,
        COUNT(DISTINCT usrl.Id) AS TotalUserCount
    FROM 
        [Role] rl
    LEFT JOIN 
        [RoleAccess] rla ON rla.RoleId = rl.Id AND (rla.CanCreate = 1 OR rla.CanModify = 1 OR rla.CanDelete = 1 OR rla.CanRead = 1)
    LEFT JOIN 
        UserRole usrl ON usrl.RoleId = rl.Id
    LEFT JOIN
	    Module mdl ON rla.ModuleId = mdl.Id
		WHERE mdl.DateDeleted IS NULL
    GROUP BY 
        rl.Id,
        rl.[Name],
        rl.[Description],
        rl.DateCreated,
        rl.DateModified,
        rl.IsDisabled,
        rl.IsLocked;
END
