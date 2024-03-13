CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetAll]
 
AS
 SELECT 
	apl.*,
	CONCAT(u.LastName,', ',u.FirstName,'',u.MiddleName) ApplicantFullName,
	u.[Position] PositionName,  --applicant position
	'Waiting' as ApplicationStatus
 FROM ApplicantsPersonalInformation apl
 LEFT JOIN [User] u on u.Id = apl.UserId
 LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
 LEFT JOIN [Role] r ON r.Id = ur.RoleId
RETURN 0
