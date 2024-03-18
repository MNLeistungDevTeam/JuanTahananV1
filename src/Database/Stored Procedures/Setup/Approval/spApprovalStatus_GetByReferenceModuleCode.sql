﻿CREATE PROCEDURE [dbo].[spApprovalStatus_GetByReferenceModuleCode]
@referenceId INT,
@moduleCode NVARCHAR(255),
@companyId INT
AS
	
   
	SELECT
		aps.Id,
		 apl.DateUpdated,
		 (Select Count(*) from ModuleStage WHERE vw.ModuleId = ModuleId) MaxStageLevel,
		  ROW_NUMBER() OVER (PARTITION BY vw.ReferenceId ORDER BY apl.DateUpdated DESC) AS ApprovalLevelId,
		 vw.*
		 FROM vwTransactions vw
	LEFT JOIN ApprovalStatus aps ON aps.ReferenceId = vw.ReferenceId and aps.ReferenceType = vw.ModuleId
	LEFT JOIN ApprovalLevel apl ON apl.ApprovalStatusId = aps.Id
	WHERE vw.ReferenceId = @referenceId AND vw.ModuleCode = @moduleCode AND vw.CompanyId =COALESCE (@companyId,vw.CompanyId)
RETURN 0