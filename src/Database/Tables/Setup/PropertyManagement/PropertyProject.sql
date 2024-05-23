CREATE TABLE [dbo].[PropertyProject]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(244) NULL,
	[Description] NVARCHAR(244) NULL,
	[Logo] NVARCHAR(244) NULL,
	[CompanyId] INT NULL,
	[CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL DEFAULT GETDATE(), 
	[ProfileImage] NVARCHAR(255) NULL
)
