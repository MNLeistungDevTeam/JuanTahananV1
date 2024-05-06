﻿CREATE TABLE [dbo].[PropertyLocation]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(244) NULL,
	[CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL DEFAULT GETDATE(), 
)
