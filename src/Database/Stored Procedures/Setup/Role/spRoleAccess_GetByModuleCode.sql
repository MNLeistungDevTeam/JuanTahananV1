 CREATE PROCEDURE [dbo].[spRoleAccess_GetByModuleCode]
	@userId INT,
	@moduleCode NVARCHAR(50)
AS
BEGIN
 	SELECT 
		roleAccess.*,
		module.Id ModuleId,
		module.[Description] ModuleName,
		module.[Code] ModuleCode,
		module.Icon ModuleIcon,
		module.Controller ModuleController,
		module.[Action] ModuleAction,
		module.[Ordinal] ModuleOrder,
		module.[IsVisible],
		module.ParentModuleId,
		CASE
			WHEN withParentModule.ParentModuleId IS NOT NULL THEN 1 
			ELSE 0 
		END HasSubModule,
		CASE
			WHEN roleAccess.CanCreate = 1 AND roleAccess.CanModify = 1 AND roleAccess.CanDelete = 1 THEN 'View and Edit ' + module.[Description]
			WHEN (roleAccess.CanCreate = 1 OR roleAccess.CanModify = 1) AND roleAccess.CanDelete = 0 THEN 'Create/Edit ' + module.[Description]
			WHEN roleAccess.CanCreate = 0 AND roleAccess.CanRead = 1 THEN 'View ' + module.[Description]
			ELSE ''
		END AccessString
	FROM Module module
	LEFT JOIN (
		SELECT ra.*
		FROM RoleAccess ra
		INNER JOIN [Role] r ON r.Id = ra.RoleId
         INNER JOIN UserRole ur on ur.RoleId = ra.RoleId 
		 INNER JOIN [User] u on u.Id = ur.UserId
		WHERE u.Id = @userId
	) roleAccess ON module.Id = roleAccess.ModuleId OR roleAccess.Id IS NULL
	LEFT JOIN (
		SELECT 
			DISTINCT 
			ParentModuleId 
		FROM Module 
		WHERE ParentModuleId <> NULL OR ParentModuleId <> 0
	) withParentModule ON withParentModule.ParentModuleId = module.Id
 
	WHERE module.Code = @moduleCode
	ORDER BY
		module.[Ordinal];
END