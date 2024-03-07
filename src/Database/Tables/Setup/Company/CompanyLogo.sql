CREATE TABLE [dbo].[CompanyLogo] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Description]  NVARCHAR (255) NOT NULL,
    [Location]     NVARCHAR (255) NOT NULL,
    [IsDisabled]   BIT            DEFAULT ((0)) NOT NULL,
    [CreatedById]    INT            NOT NULL,
    [DateCreated]  DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedById]   INT          NULL,
    [DateModified] DATETIME2 (7)  NULL,
    [CompanyId]    INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CompanyLogo_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([Id])
);