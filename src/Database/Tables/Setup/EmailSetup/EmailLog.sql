CREATE TABLE [dbo].[EmailLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReferenceId] INT NOT NULL, 
    [ReferenceNo] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(255) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL, 
    [SenderId] INT NOT NULL, 
    [ReceiverId] INT NOT NULL,
    [Date] DATETIME2 NOT NULL DEFAULT GETDATE()

)
