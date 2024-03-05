CREATE PROCEDURE [dbo].[spUserRole_GetAllRoles]

AS
	BEGIN
	SELECT
		  usrl.Id,
		  usrl.RoleId,
		  usrl.UserId,
		  usr.FirstName,
		  usr.MiddleName,
		  usr.LastName,
		  usr.Email,
		  usr.Position,
		  usr.DateCreated AS JoinedDate,
		  usr.LastOnlineTime AS [Status],
		  usr.ProfilePicture
		  FROM [UserRole] usrl
		  LEFT JOIN [User] usr ON usr.Id = usrl.UserId
END
