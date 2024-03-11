 CREATE TABLE [dbo].[NotificationPriorityLevel] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [LevelName] NVARCHAR (100) NOT NULL,
    DateCreated  DATETIME2(7) DEFAULT GETDATE(),
    CreatedById INT,
    DateModified DATETIME2(7),
    ModifiedById INT
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

