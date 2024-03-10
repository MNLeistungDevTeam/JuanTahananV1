CREATE TABLE [dbo].[AddressType]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Code] NVARCHAR(50) NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL,
    [CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT  NULL, 
    [DateModified] DATETIME2  NULL 
)
