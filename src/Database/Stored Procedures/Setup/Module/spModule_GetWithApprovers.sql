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
		END ApprovalRouteType, moduleType.Ordinal ModuleTypeOrder,
		ModuleStageCount
	FROM Module module
	LEFT JOIN Module parentModule ON parentModule.Id = module.ParentModuleId
	LEFT JOIN ModuleType moduleType ON moduleType.Id = module.ModuleTypeId
	LEFT JOIN (
	 
 
				
			 SELECT
				m.[Description] AS ModuleDescription,
				m.Id AS ModuleId,
				COUNT(ms.Id) AS ModuleStageCount
			FROM
				Module m
			LEFT JOIN
				ModuleStage ms ON m.Id = ms.ModuleId
			LEFT JOIN
				ModuleStageApprover msa ON msa.ModuleStageId = ms.Id
			GROUP BY
				m.[Description],
				m.Id
			HAVING
				COUNT(msa.Id) <> 0

	
	
	)msc ON msc.ModuleId = module.Id


	WHERE module.WithApprover = 1
RETURN 0 
 
 
 

 
