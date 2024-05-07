CREATE TABLE [dbo].[BuyerConfirmation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	UserId INT NOT NULL,
	Code NVARCHAR(255) NOT NULL,
	PagibigNumber NVARCHAR(50) NULL,
	ProjectProponentName NVARCHAR(150) NULL,
	JuridicalPersonalityId INT NULL,
	LastName NVARCHAR(100) NULL,
	FirstName NVARCHAR(100) NULL,
	MiddleName NVARCHAR(100) NULL,
	Suffix NVARCHAR(50) NULL,
	BirthDate DATETIME2(7),
	MothersMaidenName NVARCHAR(255) NULL,
	MaritalStatus NVARCHAR(100) NULL,
	OccupationStatus NVARCHAR(100) NULL,
	[PresentUnitName] NVARCHAR(255) NULL,
    [PresentBuildingName] NVARCHAR(255) NULL,
    [PresentLotName] NVARCHAR(255) NULL,
    [PresentStreetName] NVARCHAR(255) NULL,
    [PresentSubdivisionName] NVARCHAR(255) NULL,
    [PresentBaranggayName] NVARCHAR(255) NULL,
    [PresentMunicipalityName] NVARCHAR(255) NULL,
    [PresentProvinceName] NVARCHAR(255) NULL,
    [PresentZipCode] NVARCHAR(255) NULL,
    HomeNumber NVARCHAR(50) NULL,
    MobileNumber NVARCHAR(50) NULL,
    BusinessTelNo NVARCHAR(50) NULL,
    Email NVARCHAR(255) NULL,
	CompanyEmployerName  NVARCHAR(255) NULL,
	CompanyEmployerAddress  NVARCHAR(255) NULL,


	SpouseLastName  NVARCHAR(100) NULL,
	SpouseFirstName  NVARCHAR(100) NULL,
	SpouseMiddleName  NVARCHAR(100) NULL,
	SpouseSuffix  NVARCHAR(50) NULL,
	SpouseCompanyEmployerName  NVARCHAR(255) NULL,
	SpouseCompanyEmployerAddress  NVARCHAR(255) NULL,

	SpouseMonthlySalary Decimal(7,2) NULL,
	 MonthlySalary Decimal(7,2) NULL,


	OtherSourceofIncome Decimal(7,2) NULL,
	AdditionalSourceIncome Decimal(7,2) NULL,

	AverageMonthlyAdditionalIncome Decimal(7,2) NULL,
	AffordMonthlyAmortization Decimal(7,2) NULL,
	
	IsPagibigMember  bit default(0),
	IsPagibigCoBorrower  bit default(0),
	IsPursueProjectProponent  bit default(0),
	IsInformedTermsConditions  bit default(0),
	 IsPagibigAvailedLoan  bit default(0),

	

	 
	HouseUnitModel  NVARCHAR(255) NULL,
	SellingPrice Decimal(7,2) NULL,
	MonthlyAmortization Decimal(7,2) NULL,

	DateCreated DATETIME2(7) DEFAULT(GETDATE()),
	CreatedById INT NULL,
	DateModified DATETIME2(7) NULL,
	ModifiedById INT NULL


)
