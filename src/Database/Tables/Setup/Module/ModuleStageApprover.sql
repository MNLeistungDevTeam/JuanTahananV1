CREATE TABLE [dbo].[ModuleStageApprover]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ModuleStageId] INT NOT NULL, 
    [ApproverId] INT  NULL, 
    [RoleId] INT NULL,
    [IsDisabled] BIT NOT NULL DEFAULT 0, 
    [CreatedById] INT NOT NULL, 
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETDATE(), 
    [ModifiedById] INT NULL, 
    [DateModified] DATETIME2(7) NULL DEFAULT GETDATE()
)