
 Create Table SourcePagibigFund 
 (
 Id INT NOT NULL PRIMARY KEY IDENTITY,
 [Name] NVARCHAR(255) NOT NULL,
 DateCreated DATETIME2(7) NULL,
 CreatedById INT NOT NULL,
 DateModified DATETIME2(7) NULL,
 ModifiedById INT NULL
)


