CREATE PROCEDURE [dbo].[spUser_GetByRoleName]
	@roleName NVARCHAR(50)
AS
 BEGIN
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
        ap.Code ApplicantCode
    FROM 
        [User] usr
    LEFT JOIN 
        (SELECT DISTINCT UserId,RoleId FROM UserRole) usrl ON usrl.UserId = usr.Id
    LEFT JOIN 
        [Role] r ON r.Id = usrl.RoleId
    LEFT JOIN (
        SELECT UserId, COUNT(Id) AS TotalLoanCounts,Code
        FROM ApplicantsPersonalInformation
        GROUP BY UserId,Code
    ) ap ON ap.UserId = usr.Id
    WHERE 
        r.[Name] = @roleName
		ORDER BY usr.DateCreated DESC;
END
