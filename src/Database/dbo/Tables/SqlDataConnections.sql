CREATE TABLE [dbo].[SqlDataConnections] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [DisplayName]      NVARCHAR (MAX) NOT NULL,
    [ConnectionString] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_SqlDataConnections] PRIMARY KEY CLUSTERED ([Id] ASC)
);

