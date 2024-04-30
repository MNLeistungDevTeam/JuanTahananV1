CREATE PROCEDURE [dbo].[spUser_GetByRoleName]
	@roleName NVARCHAR(50),
    @companyId INT
AS
 BEGIN
       
     WITH RankedCodes AS (
    SELECT 
        Code,
        UserId,
        ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY CASE WHEN ApprovalStatus = 1 THEN 1 ELSE 0 END DESC) AS CodeRank
    FROM 
        ApplicantsPersonalInformation 
    WHERE 
        ApprovalStatus IN (0,1,3,4,6,7,8,11)
)

SELECT
    usr.Id,
    usr.UserName,
    usr.DateCreated,
    usr.LastOnlineTime,
    usr.Position,
    usr.ProfilePicture,
    usr.Email,
    usr.FirstName,
    usr.MiddleName,
    usr.LastName,
    COALESCE(ap.TotalLoanCounts, 0) AS TotalLoanCounts,
    usr.PagibigNumber,
    RankedCodes.Code AS ActiveApplicationCode
FROM 
    [User] usr
LEFT JOIN 
    (SELECT DISTINCT UserId, RoleId FROM UserRole) usrl ON usrl.UserId = usr.Id
LEFT JOIN 
    [Role] r ON r.Id = usrl.RoleId
LEFT JOIN (
    SELECT UserId, COUNT(Id) AS TotalLoanCounts
    FROM ApplicantsPersonalInformation
    WHERE CompanyId = @companyId
    GROUP BY UserId
) ap ON ap.UserId = usr.Id
LEFT JOIN RankedCodes ON RankedCodes.UserId = usr.Id AND RankedCodes.CodeRank = 1
WHERE 
    r.[Name] = @roleName
ORDER BY usr.DateCreated DESC;

END
