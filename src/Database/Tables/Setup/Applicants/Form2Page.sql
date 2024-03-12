CREATE TABLE [dbo].[Form2Page]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ApplicantsPersonalInformationId] INT NOT NULL,

    --Bank Account
    [Bank1] NVARCHAR(255) NULL,
    [Bank2] NVARCHAR(255) NULL,
    [Bank3] NVARCHAR(255) NULL,

    [BranchAddress1] NVARCHAR(255) NULL,
    [BranchAddress2] NVARCHAR(255) NULL,
    [BranchAddress3] NVARCHAR(255) NULL,

    [TypeOfAccount1] NVARCHAR(255) NULL,
    [TypeOfAccount2] NVARCHAR(255) NULL,
    [TypeOfAccount3] NVARCHAR(255) NULL,

    [AccountNumber1] BIGINT NULL,
    [AccountNumber2] BIGINT NULL,
    [AccountNumber3] BIGINT NULL,

    
    [DateOpened1] DATETIME2 NULL,
    [DateOpened2] DATETIME2 NULL,
    [DateOpened3] DATETIME2 NULL,

    [AverageBalance1] DECIMAL(18,2) NULL,
    [AverageBalance2] DECIMAL(18,2) NULL,
    [AverageBalance3] DECIMAL(18,2) NULL,

    --Credit Card Owned
    [IssuerName1] NVARCHAR(255) NULL,
    [IssuerName2] NVARCHAR(255) NULL,
    [IssuerName3] NVARCHAR(255) NULL,

    [CardType1] NVARCHAR(255) NULL,
    [CardType2] NVARCHAR(255) NULL,
    [CardType3] NVARCHAR(255) NULL,

    [CardExpiration1] DATETIME2 NULL,
    [CardExpiration2] DATETIME2 NULL,
    [CardExpiration3] DATETIME2 NULL,

    [CreditLimit1] DECIMAL(18,2) NULL,
    [CreditLimit2] DECIMAL(18,2) NULL,
    [CreditLimit3] DECIMAL(18,2) NULL,

    --Real Estate Owned

    [Location1] NVARCHAR(255) NULL,
    [Location2] NVARCHAR(255) NULL,
    [Location3] NVARCHAR(255) NULL,

    [TypeOfProperty1] NVARCHAR(255) NULL,
    [TypeOfProperty2] NVARCHAR(255) NULL,
    [TypeOfProperty3] NVARCHAR(255) NULL,

    [AquisitionCost1] DECIMAL(18,2) NULL,
    [AquisitionCost2] DECIMAL(18,2) NULL,
    [AquisitionCost3] DECIMAL(18,2) NULL,

    [MarketValue1] DECIMAL(18,2) NULL,
    [MarketValue2] DECIMAL(18,2) NULL,
    [MarketValue3] DECIMAL(18,2) NULL,

    [MortgageBalance1] DECIMAL(18,2) NULL,
    [MortgageBalance2] DECIMAL(18,2) NULL,
    [MortgageBalance3] DECIMAL(18,2) NULL,

    [RentalIncome1] DECIMAL(18,2) NULL,
    [RentalIncome2] DECIMAL(18,2) NULL,
    [RentalIncome3] DECIMAL(18,2) NULL,

    --Outstanding Credit Loan
    [CreditorAndAddress1] NVARCHAR(255) NULL,
    [CreditorAndAddress2] NVARCHAR(255) NULL,
    [CreditorAndAddress3] NVARCHAR(255) NULL,

    [Security1] NVARCHAR(255) NULL,
    [Security2] NVARCHAR(255) NULL,
    [Security3] NVARCHAR(255) NULL,

    [Type1] NVARCHAR(255) NULL,
    [Type2] NVARCHAR(255) NULL,
    [Type3] NVARCHAR(255) NULL,

    [AmountBalance1] DECIMAL(18,2) NULL,
    [AmountBalance2] DECIMAL(18,2) NULL,
    [AmountBalance3] DECIMAL(18,2) NULL,


    [MaturityDateTime1] DATETIME2 NULL,
    [MaturityDateTime2] DATETIME2 NULL,
    [MaturityDateTime3] DATETIME2 NULL,

    [Amortization1] DECIMAL(18,2) NULL,
    [Amortization2] DECIMAL(18,2) NULL,
    [Amortization3] DECIMAL(18,2) NULL,

    --Misclananous
    [PendingCase] NVARCHAR(MAX) NULL,
    [PastDue] NVARCHAR(MAX) NULL,
    [BouncingChecks] NVARCHAR(MAX) NULL,
    [MedicalAdvice] NVARCHAR(MAX) NULL,


    --loan Credit reference
    [BankFinancial1] NVARCHAR(255) NULL,
    [BankFinancial2] NVARCHAR(255) NULL,
    [BankFinancial3] NVARCHAR(255) NULL,

    [Address1] NVARCHAR(MAX) NULL,
    [Address2] NVARCHAR(MAX) NULL,
    [Address3] NVARCHAR(MAX) NULL,

    [Purpose1] NVARCHAR(255) NULL,
    [Purpose2] NVARCHAR(255) NULL,
    [Purpose3] NVARCHAR(255) NULL,


    [LoanSecurity1] NVARCHAR(255) NULL,
    [LoanSecurity2] NVARCHAR(255) NULL,
    [LoanSecurity3] NVARCHAR(255) NULL,

    [HighestAmount1] DECIMAL(18,2) NULL,
    [HighestAmount2] DECIMAL(18,2) NULL,
    [HighestAmount3] DECIMAL(18,2) NULL,


    [PresentBalance1] DECIMAL(18,2) NULL,
    [PresentBalance2] DECIMAL(18,2) NULL,
    [PresentBalance3] DECIMAL(18,2) NULL,

    [DateObtained1] DATETIME2 NULL,
    [DateObtained2] DATETIME2 NULL,
    [DateObtained3] DATETIME2 NULL,

    [DateFullyPaid1] DATETIME2 NULL,
    [DateFullyPaid2] DATETIME2 NULL,
    [DateFullyPaid3] DATETIME2 NULL,

    --TradeReferences
    [NameSupplier1] NVARCHAR(255) NULL,
    [NameSupplier2] NVARCHAR(255) NULL,
    [NameSupplier3] NVARCHAR(255) NULL,

    [TradeAddress1] NVARCHAR(255) NULL,
    [TradeAddress2] NVARCHAR(255) NULL,
    [TradeAddress3] NVARCHAR(255) NULL,

    [TradeTellNo1] BIGINT NULL,
    [TradeTellNo2] BIGINT NULL,
    [TradeTellNo3] BIGINT NULL,

    --character Reference
    [CharacterNameSupplier1] NVARCHAR(255) NULL,
    [CharacterNameSupplier2] NVARCHAR(255) NULL,
    [CharacterNameSupplier3] NVARCHAR(255) NULL,

    [CharacterAddress1] NVARCHAR(255) NULL,
    [CharacterAddress2] NVARCHAR(255) NULL,
    [CharacterAddress3] NVARCHAR(255) NULL,

    [CharacterTellNo1] BIGINT NULL,
    [CharacterTellNo2] BIGINT NULL,
    [CharacterTellNo3] BIGINT NULL,

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
