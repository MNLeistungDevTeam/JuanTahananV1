﻿CREATE TABLE [dbo].[Module]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [ModuleStatusId] INT NOT NULL DEFAULT ((1)), 
    [Ordinal] INT NULL, 
    [ParentModuleId] INT NULL, 
    [Code] NVARCHAR(255) NOT NULL,
    [BreadName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    [Icon] NVARCHAR(255) NOT NULL,
    [Controller] NVARCHAR(255) NULL,
    [Action] NVARCHAR(255) NULL,
    [IsVisible] BIT NOT NULL DEFAULT ((0)), 
    [IsBreaded] BIT NOT NULL DEFAULT ((0)), 
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL, 
)
