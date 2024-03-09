CREATE PROCEDURE [dbo].[spLoanParticularsInformation_GetByApplicantId]
	@applicantId INT
AS
	SELECT  * FROM LoanParticularsInformation WHERE ApplicantsPersonalInformationId = @applicantId
RETURN 0
