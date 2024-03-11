CREATE PROCEDURE [dbo].[spModule_GetAllUserModules]
    @userId int
AS
BEGIN
    SELECT
        mdl.*,
        rla.CanCreate,
        rla.CanDelete,
        rla.CanModify,
        rla.CanRead
    FROM 
        [Module] mdl
    LEFT JOIN 
        RoleAccess rla ON rla.ModuleId = mdl.Id
    LEFT JOIN 
        UserRole usrl ON usrl.RoleId = rla.RoleId
        WHERE 
        usrl.UserId = @userId 
        AND (rla.CanCreate = 1 OR rla.CanDelete = 1 OR rla.CanModify = 1 OR rla.CanRead = 1) AND 
        mdl.DateDeleted IS NULL
END
