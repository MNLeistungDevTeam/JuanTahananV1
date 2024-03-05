CREATE TABLE [dbo].[Reports] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX)  NOT NULL,
    [DisplayName] NVARCHAR (MAX)  NOT NULL,
    [LayoutData]  VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED ([Id] ASC)
);

