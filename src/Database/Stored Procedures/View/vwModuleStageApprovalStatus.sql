 CREATE VIEW [dbo].[vwModuleStageApprovalStatus]
	AS
	-- GET ALL Module Stages with Approval Status
	WITH cte AS (
		SELECT ms1.*, al.Id ApprovalLevelId, aps.Id ApprovalStatusId,
		al.[Status] LevelStatus,
		CASE 
			WHEN al.Id IS NULL THEN ROW_NUMBER() OVER (PARTITION BY ms1.ModuleId, al.Id, aps.Id ORDER BY ms1.[Level])
			ELSE ROW_NUMBER() OVER (PARTITION BY ms1.ModuleId ORDER BY ms1.[Level])
		END row_num
		FROM ApprovalStatus aps
		LEFT JOIN ModuleStage ms1 ON aps.ReferenceType = ms1.ModuleId
		LEFT JOIN ApprovalLevel al ON aps.Id = al.ApprovalStatusId AND ms1.Id = al.ModuleStageId
	)

	SELECT * FROM cte
