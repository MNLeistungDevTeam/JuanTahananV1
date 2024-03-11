CREATE TABLE [dbo].[Notification]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Title] NVARCHAR(100) NOT NULL, 
    [Content] NVARCHAR(MAX) NOT NULL, 
    [Preview] NVARCHAR(255) NOT NULL, 
    [ActionLink] NVARCHAR(255) NOT NULL, 
    [IsRead] BIT  DEFAULT ((0)) NOT NULL,
    [DateCreated] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [CompanyId] INT NOT NULL DEFAULT 0, 
    [SenderId] INT NOT NULL, 
    [PriorityLevel] INT NOT NULL, 
    [NotificationType] INT NOT NULL
)
