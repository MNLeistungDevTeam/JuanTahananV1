
 -- =============================================

 -- Create date: <3/18/2024>v 1:32 PM
-- Description:	<Adjusted For juantahanan>

-- Adjusted By : David

CREATE VIEW [dbo].[vwTransactions]
	AS 

WITH cte AS
	(
		 

		 	--api.Code TransactionNo,
		--api.Id RecordId,
		--api.DateCreated TransactionDate,
		--api.DateCreated DatePrepared,
		--api.CompanyId,

		SELECT
			py.Id ReferenceId, py.Code ReferenceNo, (SELECT Id FROM Module WHERE Code = 'APLCNTREQ') ModuleId,
			py.CompanyId, py.CreatedById, py.DateCreated, py.ModifiedById, py.DateModified
		FROM
			ApplicantsPersonalInformation py


			UNION ALL 

			SELECT
			bc.Id ReferenceId, bc.Code ReferenceNo, (SELECT Id FROM Module WHERE Code = 'BCF-APLRQST') ModuleId,
			bc.CompanyId, bc.CreatedById, bc.DateCreated, bc.ModifiedById, bc.DateModified
		FROM
			BuyerConfirmation bc


						UNION ALL 

				SELECT
			bcd.Id ReferenceId, d.Code ReferenceNo, (SELECT Id FROM Module WHERE Code = 'BCF-UPLOAD') ModuleId,
			d.CompanyId, bcd.CreatedById, bcd.DateCreated, bcd.ModifiedById, bcd.DateModified
		FROM
			BuyerConfirmationDocument bcd
			LEFT JOIN Document d On d.ReferenceNo  = bcd.ReferenceNo and d.DocumentTypeId  = 0
		 

		 
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
 

 