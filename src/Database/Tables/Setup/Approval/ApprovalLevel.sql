CREATE TABLE [dbo].[ApprovalLevel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ApprovalStatusId] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [ApproverId] INT NOT NULL, 
    [Level] INT NOT NULL, 
    [Remarks] NVARCHAR(MAX) NULL, 
    [DateUpdated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [ModuleStageId] INT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ApprovalLevel',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ApprovalLevel',
    @level2type = N'COLUMN',
    @level2name = N'Level'