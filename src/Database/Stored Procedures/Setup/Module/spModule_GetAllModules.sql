CREATE PROCEDURE [dbo].[spModule_GetAllModules]

AS
BEGIN
	SELECT 
		  mdl.Id,
		  mdl.IsVisible,
		  mdl.Code,
		  mdl.BreadName,
		  mdl.[Description],
		  mdl.Controller,
		  mdl.[Action],
		  mdl.Icon,
		  mdl.Ordinal,
		  mdl.IsBreaded,
		  mdl.ParentModuleId,
		  mdl_sub.[Description] AS ParentModuleName,
		  mdl.ModuleStatusId,
		  mdlsts.[Description] AS StatusName,
		  mdlsts.Color AS StatusColor,
		  usrc.UserName AS CreatedBy,
		  usrm.UserName AS ModifiedBy,
		  mdl.DateCreated,
		  mdl.DateModified
	FROM Module mdl
	LEFT JOIN [User] usrc ON usrc.Id = mdl.CreatedById
	LEFT JOIN [User] usrm ON usrm.Id = mdl.ModifiedById
	LEFT JOIN [ModuleStatus] mdlsts ON mdlsts.Id = mdl.ModuleStatusId
	LEFT JOIN Module mdl_sub ON mdl_sub.Id = mdl.ParentModuleId
	WHERE mdl.DateDeleted IS NULL
END