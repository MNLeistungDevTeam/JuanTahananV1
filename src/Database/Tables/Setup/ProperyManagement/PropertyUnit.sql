CREATE TABLE [dbo].[PropertyUnit]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(244) NULL,
	[Description] NVARCHAR(244) NULL,
	[CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL DEFAULT GETDATE(), 
)
