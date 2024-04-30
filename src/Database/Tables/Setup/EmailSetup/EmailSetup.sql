CREATE TABLE [dbo].[EmailSetup]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Host NVARCHAR(255) NOT NULL,
    DisplayName NVARCHAR(255) NULL,
    Port INT NOT NULL,
    CompanyId INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GetDate(), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    IsDefault BIT NOT NULL
)
