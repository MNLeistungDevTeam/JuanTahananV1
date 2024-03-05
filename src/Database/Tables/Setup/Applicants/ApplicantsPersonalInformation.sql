CREATE TABLE [dbo].[ApplicantsPersonalInformation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Code] NVARCHAR(MAX) NOT NULL,
	[UserId] INT NOT NULL,
    [PagibigNumber] BIGINT NULL,
	[HousingAccountNumber] BIGINT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL,
    CompanyId INT NULL
)
