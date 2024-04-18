CREATE TABLE [dbo].[DocumentVerification]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	DocumentTypeId INT NULL,   ----If 1 Verification Eligibility, 2 Application
	[Type] INT NOT NULL,
	CreatedById INT NULL,
	DateCreated DATETIME2(7) NULL,
	ModifiedById INT NULL,
	DateModified DATETIME2(7) NULL,
	--FOREIGN KEY (DocumentTypeId) REFERENCES DocumentType(Id) ON DELETE CASCADE
)
