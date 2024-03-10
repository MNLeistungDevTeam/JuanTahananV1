CREATE TABLE [dbo].[LoanParticularsInformation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ApplicantsPersonalInformationId] INT NOT NULL,
	[RepricingPeriod] INT NOT NULL,
    [PurposeOfLoanId] INT NOT NULL,
	[ModeOfPaymentId] INT NOT NULL,
    [DesiredLoanTermYears] INT NOT NULL,
	[DesiredLoanAmount] DECIMAL(18, 2) NOT NULL,
    [ExistingHousingApplicationNumber] NVARCHAR(255) NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL
)