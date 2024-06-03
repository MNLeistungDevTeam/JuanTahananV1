CREATE TABLE [dbo].[LockedTransaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ModuleId] INT NULL, 
    [TransactionNo] NVARCHAR(50) NOT NULL, 
    [TransactionId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [LastActivity] DATETIME2 NOT NULL DEFAULT GETDATE()
)
