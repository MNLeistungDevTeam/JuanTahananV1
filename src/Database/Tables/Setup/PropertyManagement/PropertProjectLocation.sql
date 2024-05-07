CREATE TABLE [dbo].[PropertyProjectLocation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ProjectId] INT NOT NULL,
	[LocationId] INT NOT NULL,
	[CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL DEFAULT GETDATE(), 
)
