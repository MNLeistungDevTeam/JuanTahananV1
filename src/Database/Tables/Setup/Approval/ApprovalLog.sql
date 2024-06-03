CREATE TABLE [dbo].[ApprovalLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ReferenceId] INT NOT NULL,  --ApprovalStatusId
    [StageId] INT NOT NULL, --ModuleStageId
    [Action] INT NOT NULL, 
    [Comment] NVARCHAR(255) NULL, 
    [CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL, 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ApprovalLevelId] INT NULL,
    [ApprovalStatusId] INT NULL 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Transaction Record Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalLog',
    @level2type = N'COLUMN',
    @level2name = N'ReferenceId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Module Stage Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalLog',
    @level2type = N'COLUMN',
    @level2name = N'StageId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'1 = Approved, 2 = Rejected, 3 = Cancelled',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalLog',
    @level2type = N'COLUMN',
    @level2name = N'Action'