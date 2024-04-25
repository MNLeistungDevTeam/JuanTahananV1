CREATE TABLE [dbo].[AuditTrailRef]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ColumnName] NVARCHAR(50) NULL, 
    [ColumnDisplay] NVARCHAR(50) NULL
)
