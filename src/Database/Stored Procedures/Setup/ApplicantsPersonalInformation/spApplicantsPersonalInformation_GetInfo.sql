CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetInfo]
	@roleId INT,
	@pagibigNumber NVARCHAR(50)

--WITHOUT PERCENTAGE
AS 

	SET NOCOUNT ON;

    SELECT
        COUNT(apl.Id) AS TotalApplication,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 0 THEN 1 ELSE 0 END), 0) AS Draft,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 1 THEN 1 ELSE 0 END), 0) AS Submitted,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 2 THEN 1 ELSE 0 END), 0) AS Deferred,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 3 THEN 1 ELSE 0 END), 0) AS DeveloperVerified,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 4 THEN 1 ELSE 0 END), 0) AS PagIbigVerified,
        COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 5 THEN 1 ELSE 0 END), 0) AS Withdrawn,
	    COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 6 THEN 1 ELSE 0 END), 0) AS PostSubmitted,
	    COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 7 THEN 1 ELSE 0 END), 0) AS DeveloperConfirmed,
	    COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 8 THEN 1 ELSE 0 END), 0) AS PagibigConfirmed,
		COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 9 THEN 1 ELSE 0 END), 0) AS Disqualified,
	    COALESCE(SUM(CASE WHEN apl.ApprovalStatus = 10 THEN 1 ELSE 0 END), 0) AS Discontinued

    FROM ApplicantsPersonalInformation apl
    LEFT JOIN [User] u ON u.Id = apl.UserId
    LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
    LEFT JOIN [Role] r ON r.Id = ur.RoleId
    WHERE
        @roleId = 1 OR -- Ignore pagibigNumber when roleId is 1
        (
            (@pagibigNumber IS NOT NULL AND apl.PagIbigNumber = @pagibigNumber) AND
            (
                (@roleId = 2 AND apl.ApprovalStatus IN (3, 4, 5)) OR -- LGU
                (@roleId = 4 AND apl.ApprovalStatus IN (0, 1, 2, 3, 4, 5)) OR -- Beneficiary
                (@roleId = 5 AND apl.ApprovalStatus IN (1, 2, 3, 4, 5)) OR -- Developer
                (@roleId = 3 AND apl.ApprovalStatus IN (2, 3, 4, 5)) -- Pagibig
            )
        );

RETURN 0