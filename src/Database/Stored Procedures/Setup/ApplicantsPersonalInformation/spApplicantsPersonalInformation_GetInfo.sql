CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetInfo]
	@roleId INT

--WITHOUT PERCENTAGE
AS 
	SET NOCOUNT ON;
			SELECT
				COUNT(apl.Id) AS TotalApplication,
				COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 0 THEN 1 ELSE 0 END), 0) AS ApplicationDraft,
				COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 1 THEN 1 ELSE 0 END), 0) AS Submitted,
				COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 3 THEN 1 ELSE 0 END), 0) AS DeveloperVerified,
				COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 4 THEN 1 ELSE 0 END), 0) AS PagIbigVerified,
				COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 5 THEN 1 ELSE 0 END), 0) AS Withdrawn
			FROM ApplicantsPersonalInformation apl
			LEFT JOIN [User] u ON u.Id = apl.UserId
			LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
			LEFT JOIN [Role] r ON r.Id = ur.RoleId
			WHERE
			1 = (
				CASE  
					WHEN @roleId = 1 THEN 1 --Admin
					WHEN @roleId = 2 THEN  --LGU
						CASE WHEN apl.ApprovalStatus IN (3,4,5) THEN 1 ELSE 0 END
					WHEN @roleId = 4 THEN --Beneficiary
						CASE WHEN apl.ApprovalStatus IN (0,1,2,3,4,5) THEN 1 ELSE 0 END
					WHEN @roleId = 5 THEN --Developer 
						CASE WHEN apl.ApprovalStatus IN (1,2,3,4,5) THEN 1 ELSE 0 END
					WHEN @roleId = 3 THEN --Pagibig 
						CASE WHEN apl.ApprovalStatus IN (2,3,4,5) THEN 1 ELSE 0 END
				END
			);
RETURN 0


--WITH PERCENTAGE
--SET NOCOUNT ON;

--DECLARE @TotalCount INT;
--SELECT @TotalCount = COUNT(apl.Id)
--FROM ApplicantsPersonalInformation apl
--LEFT JOIN [User] u ON u.Id = apl.UserId
--LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
--LEFT JOIN [Role] r ON r.Id = ur.RoleId
--WHERE
--    1 = (
--        CASE  
--            WHEN @roleId = 1 THEN 1 -- Admin
--            WHEN @roleId = 2 THEN  -- LGU
--                CASE WHEN apl.ApprovalStatus IN (3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 4 THEN -- Beneficiary
--                CASE WHEN apl.ApprovalStatus IN (0,1,2,3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 5 THEN -- Developer 
--                CASE WHEN apl.ApprovalStatus IN (1,2,3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 3 THEN -- Pagibig 
--                CASE WHEN apl.ApprovalStatus IN (2,3,4,5) THEN 1 ELSE 0 END
--        END
--    );

--SELECT
--    @TotalCount AS TotalApplication,
--    (COUNT(apl.Id) * 100 / @TotalCount) AS ApplicationDraftPercentage,
--    (SUM(CASE WHEN apl.ApprovalStatus = 0 THEN 1 ELSE 0 END) * 100 / @TotalCount) AS ApplicationDraft,
--    (SUM(CASE WHEN apl.ApprovalStatus = 1 THEN 1 ELSE 0 END) * 100 / @TotalCount) AS Submitted,
--    (SUM(CASE WHEN apl.ApprovalStatus = 3 THEN 1 ELSE 0 END) * 100 / @TotalCount) AS DeveloperVerified,
--    (SUM(CASE WHEN apl.ApprovalStatus = 4 THEN 1 ELSE 0 END) * 100 / @TotalCount) AS PagIbigVerified,
--    (SUM(CASE WHEN apl.ApprovalStatus = 5 THEN 1 ELSE 0 END) * 100 / @TotalCount) AS Withdrawn
--FROM ApplicantsPersonalInformation apl
--LEFT JOIN [User] u ON u.Id = apl.UserId
--LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
--LEFT JOIN [Role] r ON r.Id = ur.RoleId
--WHERE
--    1 = (
--        CASE  
--            WHEN @roleId = 1 THEN 1 -- Admin
--            WHEN @roleId = 2 THEN  -- LGU
--                CASE WHEN apl.ApprovalStatus IN (3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 4 THEN -- Beneficiary
--                CASE WHEN apl.ApprovalStatus IN (0,1,2,3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 5 THEN -- Developer 
--                CASE WHEN apl.ApprovalStatus IN (1,2,3,4,5) THEN 1 ELSE 0 END
--            WHEN @roleId = 3 THEN -- Pagibig 
--                CASE WHEN apl.ApprovalStatus IN (2,3,4,5) THEN 1 ELSE 0 END
--        END
--    );