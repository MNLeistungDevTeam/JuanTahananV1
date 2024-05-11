CREATE PROCEDURE [dbo].[spApprovalStatus_Inquiry]
	@referenceId int,
	@companyId int,
	@approvalStatusId int,
	@referenceType int

AS

	;WITH cte AS (
		SELECT ms1.*, al.Id ApprovalLevelId, aps.Id ApprovalStatusId,
		al.[Status] ApprovalLevelStatus,
		ROW_NUMBER() OVER (PARTITION BY ms1.ModuleId, aps.Id ORDER BY
			CASE WHEN al.Id IS NULL OR al.[Status] IN (0, 1, 4) THEN 1 ELSE 2 END,
			ms1.[Level]
		) as row_num
		FROM ApprovalStatus aps
		LEFT JOIN ModuleStage ms1 ON aps.ReferenceType = ms1.ModuleId
		LEFT JOIN ApprovalLevel al ON aps.Id = al.ApprovalStatusId AND ms1.Id = al.ModuleStageId
	)

	SELECT
		vw.*,
		up.UserName PreparedBy,
		ua.Id CurrentApproverUserId,
		ua.UserName CurrentApprover,
		api.Code TransactionNo,
		api.Id RecordId,
		api.DateCreated TransactionDate,
		api.DateCreated DatePrepared,
		api.CompanyId,
		aplvl.Remarks
	FROM
		(
			SELECT
				aps.*,
				ms.ApprovalLevelId,
				CASE WHEN apl.Id IS NULL THEN msm.MinLevel
				ELSE ms.[Level] END [Level],
				m.Id ModuleId,
				m.Code ModuleCode,
				m.[Description] ModuleDescription,
				m.Controller ModuleController
			FROM
				ApprovalStatus aps
				LEFT JOIN (
					SELECT apl.*
					FROM ApprovalLevel apl
					INNER JOIN ModuleStage ms ON apl.ModuleStageId = ms.Id
					INNER JOIN (
						SELECT apl2.ApprovalStatusId, MAX(ms.[Level]) MaxLevel
						FROM ApprovalLevel apl2
						INNER JOIN ModuleStage ms ON apl2.ModuleStageId = ms.Id
						GROUP BY apl2.ApprovalStatusId
					) x ON x.ApprovalStatusId = apl.ApprovalStatusId AND ms.[Level] = x.MaxLevel
				) apl ON aps.Id = apl.ApprovalStatusId
				LEFT JOIN (
					SELECT ModuleId, COUNT(*) StageCount, MAX([Level]) MaxLevel,
					MIN([Level]) MinLevel
					FROM ModuleStage
					GROUP BY ModuleId
				) msm ON aps.ReferenceType = msm.ModuleId
				LEFT JOIN (			
					SELECT * FROM cte
					WHERE row_num = 1
				) ms ON ms.ApprovalStatusId = aps.Id
				LEFT JOIN Module m ON ms.ModuleId = m.Id
		) vw
		LEFT JOIN (
			SELECT * FROM cte
			WHERE row_num = 1
		) ms ON ms.ApprovalStatusId = vw.Id
		LEFT JOIN [User] ua ON ua.Id = ms.ApproverId
		LEFT JOIN ApplicantsPersonalInformation api ON vw.ReferenceId = api.Id And vw.ReferenceType = (Select Id from Module where Code = 'APLCNTREQ')
		LEFT JOIN [User] up ON up.Id = vw.UserId
		LEFT JOIN (
			SELECT aplvl1.*
			FROM ApprovalLevel aplvl1 
			INNER JOIN (
				SELECT ApprovalStatusId, MAX(Id) Id
				FROM ApprovalLevel
				GROUP BY ApprovalStatusId
			) maxApLvl ON aplvl1.Id = maxApLvl.Id
		) aplvl ON vw.Id = aplvl.ApprovalStatusId
		WHERE
			vw.ReferenceId = COALESCE(@ReferenceId, vw.ReferenceId)
			AND vw.ReferenceType = COALESCE(@referenceType, vw.ReferenceType)
			--1 = (
			--		CASE
			--			WHEN @ReferenceType IS NULL THEN 1
			--			WHEN vw.ReferenceType IN (SELECT value FROM string_split(@ReferenceType, ',')) THEN 1
			--			ELSE 0
			--		END	
			--)
			AND vw.Id = COALESCE(@ApprovalStatusId, vw.Id)
			--AND ua.Id = COALESCE(@ApproverId, ua.Id)
			AND api.CompanyId = COALESCE(@CompanyId, api.CompanyId)
		 

RETURN 0
