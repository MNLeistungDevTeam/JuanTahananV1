CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetInfo]
	@roleId INT,
	@pagibigNumber NVARCHAR(50)

--WITHOUT PERCENTAGE
AS 

	SET NOCOUNT ON;

    SELECT
        -- Get the count of Total Application
        COUNT(apl.Id) AS TotalApplication,
        -- Get the count of Total Submitted
        COUNT(CASE WHEN apl.ApprovalStatus = 1 THEN 1 END) AS TotalSubmitted,
        -- Get the count of Total Approved
        COUNT(CASE WHEN apl.ApprovalStatus IN (3,4,7,8) THEN 1 END) AS TotalApprove,
        -- Get the count of Total Disapprove
        COUNT(CASE WHEN apl.ApprovalStatus = 2 OR apl.ApprovalStatus = 9 THEN 1 END) AS TotalDisApprove,
        -- Get the cound of Total Withdrawn
        COUNT(CASE WHEN apl.ApprovalStatus = 5 OR apl.ApprovalStatus = 10 THEN 1 END) AS TotalWithdrawn

    FROM ApplicantsPersonalInformation apl
    LEFT JOIN [User] u ON u.Id = apl.UserId
    LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
    LEFT JOIN [Role] r ON r.Id = ur.RoleId
    WHERE
        (@roleId = 1 OR 
    (
        (@pagibigNumber IS NOT NULL AND apl.PagIbigNumber = @pagibigNumber) AND
        apl.ApprovalStatus IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
    ));

RETURN 0