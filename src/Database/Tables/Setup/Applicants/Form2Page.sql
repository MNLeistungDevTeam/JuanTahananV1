CREATE TABLE [dbo].[Form2Page]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ApplicantsPersonalInformationId] INT NOT NULL,

    --Bank Account
    [Bank1] NVARCHAR(40) NULL,
    [Bank2] NVARCHAR(40) NULL,
    [Bank3] NVARCHAR(40) NULL,

    [BranchAddress1] NVARCHAR(40) NULL,
    [BranchAddress2] NVARCHAR(40) NULL,
    [BranchAddress3] NVARCHAR(40) NULL,

    [TypeOfAccount1] NVARCHAR(40) NULL,
    [TypeOfAccount2] NVARCHAR(40) NULL,
    [TypeOfAccount3] NVARCHAR(40) NULL,

    [AccountNumber1] NVARCHAR(40) NULL,
    [AccountNumber2] NVARCHAR(40) NULL,
    [AccountNumber3] NVARCHAR(40) NULL,

    
    [DateOpened1] DATE NULL,
    [DateOpened2] DATE NULL,
    [DateOpened3] DATE NULL,

    [AverageBalance1] DECIMAL(10,2) NULL,
    [AverageBalance2] DECIMAL(10,2) NULL,
    [AverageBalance3] DECIMAL(10,2) NULL,

    --Credit Card Owned
    [IssuerName1] NVARCHAR(40) NULL,
    [IssuerName2] NVARCHAR(40) NULL,
    [IssuerName3] NVARCHAR(40) NULL,

    [CardType1] NVARCHAR(24) NULL,
    [CardType2] NVARCHAR(24) NULL,
    [CardType3] NVARCHAR(24) NULL,

    [CardExpiration1] DATE NULL,
    [CardExpiration2] DATE NULL,
    [CardExpiration3] DATE NULL,

    [CreditLimit1] DECIMAL(10,2) NULL,
    [CreditLimit2] DECIMAL(10,2) NULL,
    [CreditLimit3] DECIMAL(10,2) NULL,

    --Real Estate Owned

    [Location1] NVARCHAR(40) NULL,
    [Location2] NVARCHAR(40) NULL,
    [Location3] NVARCHAR(40) NULL,

    [TypeOfProperty1] NVARCHAR(24) NULL,
    [TypeOfProperty2] NVARCHAR(24) NULL,
    [TypeOfProperty3] NVARCHAR(24) NULL,

    [AquisitionCost1] DECIMAL(10,2) NULL,
    [AquisitionCost2] DECIMAL(10,2) NULL,
    [AquisitionCost3] DECIMAL(10,2) NULL,

    [MarketValue1] DECIMAL(10,2) NULL,
    [MarketValue2] DECIMAL(10,2) NULL,
    [MarketValue3] DECIMAL(10,2) NULL,

    [MortgageBalance1] DECIMAL(10,2) NULL,
    [MortgageBalance2] DECIMAL(10,2) NULL,
    [MortgageBalance3] DECIMAL(10,2) NULL,

    [RentalIncome1] DECIMAL(10,2) NULL,
    [RentalIncome2] DECIMAL(10,2) NULL,
    [RentalIncome3] DECIMAL(10,2) NULL,

    --Outstanding Credit Loan
    [CreditorAndAddress1] NVARCHAR(128) NULL,
    [CreditorAndAddress2] NVARCHAR(128) NULL,
    [CreditorAndAddress3] NVARCHAR(128) NULL,

    [Security1] NVARCHAR(40) NULL,
    [Security2] NVARCHAR(40) NULL,
    [Security3] NVARCHAR(40) NULL,

    [Type1] NVARCHAR(40) NULL,
    [Type2] NVARCHAR(40) NULL,
    [Type3] NVARCHAR(40) NULL,

    [AmountBalance1] DECIMAL(10,2) NULL,
    [AmountBalance2] DECIMAL(10,2) NULL,
    [AmountBalance3] DECIMAL(10,2) NULL,


    [MaturityDateTime1] DATE NULL,
    [MaturityDateTime2] DATE NULL,
    [MaturityDateTime3] DATE NULL,

    [Amortization1] DECIMAL(10,2) NULL,
    [Amortization2] DECIMAL(10,2) NULL,
    [Amortization3] DECIMAL(10,2) NULL,

    --Misclananous
    [PendingCase] NVARCHAR(255) NULL,
    [PastDue] NVARCHAR(255) NULL,
    [BouncingChecks] NVARCHAR(255) NULL,
    [MedicalAdvice] NVARCHAR(255) NULL,


    --loan Credit reference
    [BankFinancial1] NVARCHAR(40) NULL,
    [BankFinancial2] NVARCHAR(40) NULL,
    [BankFinancial3] NVARCHAR(40) NULL,

    [Address1] NVARCHAR(40) NULL,
    [Address2] NVARCHAR(40) NULL,
    [Address3] NVARCHAR(40) NULL,

    [Purpose1] NVARCHAR(40) NULL,
    [Purpose2] NVARCHAR(40) NULL,
    [Purpose3] NVARCHAR(40) NULL,


    [LoanSecurity1] NVARCHAR(40) NULL,
    [LoanSecurity2] NVARCHAR(40) NULL,
    [LoanSecurity3] NVARCHAR(40) NULL,

    [HighestAmount1] DECIMAL(10,2) NULL,
    [HighestAmount2] DECIMAL(10,2) NULL,
    [HighestAmount3] DECIMAL(10,2) NULL,


    [PresentBalance1] DECIMAL(10,2) NULL,
    [PresentBalance2] DECIMAL(10,2) NULL,
    [PresentBalance3] DECIMAL(10,2) NULL,

    [DateObtained1] DATE NULL,
    [DateObtained2] DATE NULL,
    [DateObtained3] DATE NULL,

    [DateFullyPaid1] DATE NULL,
    [DateFullyPaid2] DATE NULL,
    [DateFullyPaid3] DATE NULL,

    --TradeReferences
    [NameSupplier1] NVARCHAR(128) NULL,
    [NameSupplier2] NVARCHAR(128) NULL,
    [NameSupplier3] NVARCHAR(128) NULL,

    [TradeAddress1] NVARCHAR(40) NULL,
    [TradeAddress2] NVARCHAR(40) NULL,
    [TradeAddress3] NVARCHAR(40) NULL,

    [TradeTellNo1] NVARCHAR(25) NULL,
    [TradeTellNo2] NVARCHAR(25) NULL,
    [TradeTellNo3] NVARCHAR(25) NULL,

    --character Reference
    [CharacterNameSupplier1] NVARCHAR(128) NULL,
    [CharacterNameSupplier2] NVARCHAR(128) NULL,
    [CharacterNameSupplier3] NVARCHAR(128) NULL,

    [CharacterAddress1] NVARCHAR(40) NULL,
    [CharacterAddress2] NVARCHAR(40) NULL,
    [CharacterAddress3] NVARCHAR(40) NULL,

    [CharacterTellNo1] NVARCHAR(25) NULL,
    [CharacterTellNo2] NVARCHAR(25) NULL,
    [CharacterTellNo3] NVARCHAR(25) NULL,

    --Seller Data
    [FirstName] NVARCHAR(255) NULL,
    [MIddleName] NVARCHAR(255) NULL,
    [Suffix] NVARCHAR(255) NULL,
    [LastName] NVARCHAR(255) NULL,
    [PagibigNumber] NVARCHAR(MAX) NULL,
    [TinNumber] NVARCHAR(150) NULL,
    [ContactNumber] NVARCHAR(50) NULL,
    [Email] NVARCHAR(355) NULL,
    [SourcePagibigFundId] INT NULL,
	[DateCreated] DATETIME2 NOT NULL DEFAULT (GETDATE()), 
    [CreatedById] INT NOT NULL, 
    [DateModified] DATETIME2 NULL, 
    [ModifiedById] INT NULL,
    [DateDeleted] DATETIME2 NULL, 
    [DeletedById] INT NULL, 
    [Agreement] BIT NULL DEFAULT (1), 
    [SellersUnitName] NVARCHAR(255) NULL,
    [SellersBuildingName] NVARCHAR(255) NULL,
    [SellersLotName] NVARCHAR(255) NULL,
    [SellersStreetName] NVARCHAR(255) NULL,
    [SellersSubdivisionName] NVARCHAR(255) NULL,
    [SellersBaranggayName] NVARCHAR(255) NULL,
    [SellersMunicipalityName] NVARCHAR(255) NULL,
    [SellersProvinceName] NVARCHAR(255) NULL,
    [SellersZipCode] NVARCHAR(255) NULL,
)
