﻿CREATE TABLE [dbo].[AddressType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedBy] INT NOT NULL, 
    [DateModified] DATETIME2 NOT NULL DEFAULT GETDATE()
)
