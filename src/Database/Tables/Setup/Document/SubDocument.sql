CREATE TABLE [dbo].[SubDocument]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    DocumentTypeId INT NULL,
	[Type] INT  NULL,
	DateCreated DATETIME2(7) DEFAULT GETDATE(),
	DateModified DATETIME2(7) NULL, 
	ParentId INT NULL
	)
