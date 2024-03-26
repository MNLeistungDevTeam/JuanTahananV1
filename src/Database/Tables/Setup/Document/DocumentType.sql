CREATE TABLE [dbo].[DocumentType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Description] NVARCHAR(255) NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL, 
    [FileType] INT NULL
)
