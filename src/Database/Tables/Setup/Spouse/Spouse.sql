﻿CREATE TABLE [dbo].[Spouse]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ApplicantsPersonalInformationId] INT NOT NULL,
    [IsSpouseAddressAbroad] BIT NULL DEFAULT(0),
    [SpouseEmploymentUnitName] NVARCHAR(255) NULL,
    [SpouseEmploymentBuildingName] NVARCHAR(255) NULL,
    [SpouseEmploymentLotName] NVARCHAR(255) NULL,
    [SpouseEmploymentStreetName] NVARCHAR(255) NULL,
    [SpouseEmploymentSubdivisionName] NVARCHAR(255) NULL,
    [SpouseEmploymentBaranggayName] NVARCHAR(255) NULL,
    [SpouseEmploymentMunicipalityName] NVARCHAR(255) NULL,
    [SpouseEmploymentProvinceName] NVARCHAR(255) NULL,
    [SpouseEmploymentZipCode] NVARCHAR(255) NULL,
    [PreparedMailingAddress] NVARCHAR(255) NULL,
    [PreferredTimeToContact] DATETIME2 NULL,
    [LastName] NVARCHAR(100) NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [MiddleName] NVARCHAR(50) NULL, 
    [Suffix] NVARCHAR(50) NULL, 
    [PagibigMidNumber] NVARCHAR(MAX) NULL,
    [TinNumber] NVARCHAR(150) NULL,
    [IndustryId] INT NULL,
    [BusinessNumber] INT NULL,
    [Citizenship] VARCHAR(50) NULL, 
    [BirthDate] DATETIME2 NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL,
    BusinessName NVARCHAR(255) NULL,
    OccupationStatus NVARCHAR(100) NULL,
    YearsInEmployment INT NULL,
    EmploymentPosition NVARCHAR(255) NULL,
    BusinessTelNo NVARCHAR(50) NULL
)
