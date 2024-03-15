CREATE TABLE [dbo].[ApprovalStatus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ReferenceId] INT NOT NULL,    -- Id on Module Table
    [ReferenceType] INT NOT NULL,  --Module Id
    [Status] INT NOT NULL, 
    [LastUpdate] DATETIME2 NOT NULL DEFAULT GETDATE() 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Prepared By',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalStatus',
    @level2type = N'COLUMN',
    @level2name = 'UserId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Module Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalStatus',
    @level2type = N'COLUMN',
    @level2name = 'ReferenceType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 = For Approval, 1 = Approved, 2 = Canceled',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalStatus',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Record Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalStatus',
    @level2type = N'COLUMN',
    @level2name = N'ReferenceId'