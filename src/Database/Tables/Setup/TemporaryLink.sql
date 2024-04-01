CREATE TABLE [dbo].[TemporaryLink]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[GuId] uniqueidentifier NOT NULL,
	UserId INT NULL,
	IsSubmitted BIT DEFAULT 0,
	DateSubmitted DATETIME2(7),
	IsOpened BIT DEFAULT 0,
	DateExpiration DATETIME2(7)

)
