CREATE TABLE [dbo].[AuditTrailHeader]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(255) NULL, 
    [UserId] INT,
    [DateCreated] DATETIME2 NOT NULL
)