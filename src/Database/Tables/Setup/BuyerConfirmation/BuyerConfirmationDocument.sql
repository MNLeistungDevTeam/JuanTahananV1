CREATE TABLE [dbo].[BuyerConfirmationDocument]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	ReferenceId INT,
	ReferenceNo NVARCHAR(100),
    [Status] INT,
	Remarks NVARCHAR(255),
	CreatedById INT,
	DateCreated DATETIME2(7) DEFAULT GETDATE(),
	ModifiedById INT,
	DateModified DATETIME2(7)
)
