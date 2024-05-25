CREATE TABLE [dbo].[UserCompany]
(
	[Id] INT NOT NULL PRIMARY KEY,
	UserId INT NOT NULL,
	CompanyId INT NOT NULL,
	CreatedById INT NULL,
	DateCreated DATETIME2(7) NULL DEFAULT(GETDATE()),
	ModifiedById INT NULL,
	DateModified DATETIME2(7)
)
