CREATE PROCEDURE [dbo].[spApplicantsPersonalInformation_GetAll]
 
AS
 SELECT 
	apl.*,
	CONCAT(u.LastName,', ',u.FirstName,'',u.MiddleName) ApplicantFullName,
	u.[Position] PositionName,  --applicant position
	0.00 As IncomeAmount,
	bi.PropertyDeveloperName Developer,
	bi.PropertyLocation ProjectLocation,
	'' Project,
	bi.PropertyUnitLevelName Unit,
	lpi.DesiredLoanAmount As LoanAmount,
	CASE WHEN apl.ApprovalStatus = 1 Then 'Application in Draft'
		 WHEN  apl.ApprovalStatus = 2 Then 'Approved'
		 ELSE 'Defered'	
	END ApplicationStatus

 FROM ApplicantsPersonalInformation apl
 LEFT JOIN BarrowersInformation bi ON bi.ApplicantsPersonalInformationId = apl.Id
  LEFT JOIN LoanParticularsInformation lpi ON lpi.ApplicantsPersonalInformationId = apl.Id
 LEFT JOIN [User] u on u.Id = apl.UserId
 LEFT JOIN [UserRole] ur ON ur.UserId = u.Id
 LEFT JOIN [Role] r ON r.Id = ur.RoleId
RETURN 0