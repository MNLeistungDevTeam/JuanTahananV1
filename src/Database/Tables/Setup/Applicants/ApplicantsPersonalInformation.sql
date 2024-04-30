CREATE TABLE [dbo].[ApplicantsPersonalInformation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Code] NVARCHAR(MAX) NOT NULL,
	[UserId] INT NOT NULL,
    [PagibigNumber]  NVARCHAR(MAX) NULL,
	[HousingAccountNumber] NVARCHAR(MAX) NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL,
    CompanyId INT NULL,
    [ApprovalStatus] INT NULL DEFAULT(1),
    [EncodedStage] INT  NULL,
    [EncodedStatus] INT NULL
)
