CREATE PROCEDURE [dbo].[spModule_GetByCode]
	@code NVARCHAR(50)
AS
BEGIN
	SELECT 
		module.*,
		parentModule.[Description] AS ParentModule
	FROM Module module
	LEFT JOIN Module parentModule 
		ON parentModule.Id = module.ParentModuleId
	WHERE module.Code = @code
END