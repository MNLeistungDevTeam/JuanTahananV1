CREATE TABLE [dbo].[UserDocuments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [DocumentId] INT NOT NULL,
    [DocumentTypeId] INT NOT NULL,
    [ApplicantsPersonalInformationId] INT NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL, 
)
