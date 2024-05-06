CREATE TABLE [dbo].[PropertyUnitProject]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UnitId] INT NULL,
	[ProjectId] INT NULL,
	[CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL DEFAULT GETDATE(), 
)
