CREATE PROCEDURE [dbo].[spRoleAccess_GetByUserId]
	@userId INT
AS
BEGIN
	SELECT 
		roleAccess.*,
		module.Id ModuleId,
		module.[Description] ModuleName,
		module.[Code] ModuleCode,
		module.ModuleTypeId,
		moduleType.[Description] ModuleType,
		moduleType.Icon ModuleTypeIcon,
		moduleType.[Ordinal] ModuleTypeOrder,
		moduleType.[Controller] ModuleTypeController,
		moduleType.[Action] ModuleTypeAction,
		moduleType.[IsVisible] ModuleTypeIsVisible,
		moduleType.[IsDisabled] ModuleTypeIsDisabled,
		moduleType.[InMaintenance] ModuleTypeInMaintenance,
		module.Icon ModuleIcon,
		module.Controller ModuleController,
		module.[Action] ModuleAction,
		module.[Ordinal] ModuleOrder,
		module.IsDisabled,
		module.InMaintenance,
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
		SELECT ra.*, u.Id [UserId] 
		FROM RoleAccess ra
		INNER JOIN [Role] r ON r.Id = ra.RoleId
		INNER JOIN UserRole ur ON ur.RoleId = r.Id
		INNER JOIN [User] u ON u.id = ur.UserId
		WHERE u.Id = @userId
	) roleAccess ON module.Id = roleAccess.ModuleId AND roleAccess.UserId = @userId OR roleAccess.Id IS NULL
	LEFT JOIN ModuleType moduleType ON module.ModuleTypeId = moduleType.Id
	LEFT JOIN (
		SELECT 
			DISTINCT 
			ParentModuleId 
		FROM Module 
		WHERE ParentModuleId <> NULL OR ParentModuleId <> 0
	) withParentModule ON withParentModule.ParentModuleId = module.Id
	ORDER BY
		moduleType.[Ordinal], 
		module.[Ordinal];
END