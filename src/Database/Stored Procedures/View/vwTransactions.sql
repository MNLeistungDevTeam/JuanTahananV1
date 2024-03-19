
 -- =============================================

 -- Create date: <3/18/2024>v 1:32 PM
-- Description:	<Adjusted For juantahanan>

-- Adjusted By : David

CREATE VIEW [dbo].[vwTransactions]
	AS 

WITH cte AS
	(
		 
		SELECT
			py.Id ReferenceId, py.Code ReferenceNo, (SELECT Id FROM Module WHERE Code = 'APLCNTREQ') ModuleId,
			py.CompanyId, py.CreatedById, py.DateCreated, py.ModifiedById, py.DateModified
		FROM
			ApplicantsPersonalInformation py
		 
	)

	SELECT
		cte.*,
		m.Controller ModuleController,
		m.[Action] ModuleAction,
		m.Code ModuleCode,
		m.WithApprover
	FROM cte
	LEFT JOIN [User] [created] ON [created].Id = cte.CreatedById
	LEFT JOIN [User] [modify] ON [modify].Id = cte.ModifiedById
	LEFT JOIN Module m ON cte.ModuleId = m.Id
 