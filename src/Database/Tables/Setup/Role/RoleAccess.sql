CREATE TABLE [dbo].[RoleAccess] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [RoleId] INT NOT NULL,
    [ModuleId] INT NOT NULL,
    [CanCreate] BIT NOT NULL DEFAULT 0,
    [CanModify] BIT NOT NULL DEFAULT 0,
    [CanDelete] BIT NOT NULL DEFAULT 0,
    [CanRead] BIT NOT NULL DEFAULT 0,
    [FullAccess] BIT NOT NULL DEFAULT 0,
    [CreatedById] INT NOT NULL,
    [DateCreated] DATETIME2 (7) DEFAULT getdate() NOT NULL,
    [ModifiedById] INT NULL,
    [DateModified] DATETIME2 (7) DEFAULT getdate() NULL
);
