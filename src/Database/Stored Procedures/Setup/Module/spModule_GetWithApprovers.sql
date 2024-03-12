CREATE PROCEDURE [dbo].[spModule_GetWithApprovers]
AS
	SELECT 
		module.*,
		moduleType.[Description] ModuleType,
		parentModule.[Description] AS ParentModule,
		CASE 
			WHEN module.ApprovalRouteTypeId = 1 THEN 'Straight' 
			WHEN module.ApprovalRouteTypeId = 2 THEN 'Total Count'
			ELSE 'N/A' 
		END ApprovalRouteType, moduleType.Ordinal ModuleTypeOrder
	FROM Module module
	LEFT JOIN Module parentModule ON parentModule.Id = module.ParentModuleId
	LEFT JOIN ModuleType moduleType ON moduleType.Id = module.ModuleTypeId
	WHERE module.WithApprover = 1
RETURN 0 