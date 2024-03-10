 CREATE TABLE [dbo].[NotificationReceiver] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [NotifId]      INT           NOT NULL,
    [ReceiverId]   INT           NOT NULL,
    [ReceiverType] INT           NOT NULL,
    [IsRead]       BIT           DEFAULT ((0)) NOT NULL,
    [DateRead]     DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [IsDelete] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
     FOREIGN KEY (NotifId) REFERENCES Notification(Id) ON DELETE CASCADE
);

