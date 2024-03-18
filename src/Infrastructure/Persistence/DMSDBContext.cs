﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using DMS.Domain.Entities;
using DMS.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DMS.Infrastructure.Persistence;

public partial class DMSDBContext : DbContext
{
    public DMSDBContext(DbContextOptions<DMSDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressType> AddressTypes { get; set; }

    public virtual DbSet<ApplicantsPersonalInformation> ApplicantsPersonalInformations { get; set; }

    public virtual DbSet<ApprovalLevel> ApprovalLevels { get; set; }

    public virtual DbSet<ApprovalLog> ApprovalLogs { get; set; }

    public virtual DbSet<ApprovalStatus> ApprovalStatuses { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<BarrowersInformation> BarrowersInformations { get; set; }

    public virtual DbSet<CollateralInformation> CollateralInformations { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyLogo> CompanyLogos { get; set; }

    public virtual DbSet<CompanySetting> CompanySettings { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Form2Page> Form2Pages { get; set; }

    public virtual DbSet<JsonDataConnection> JsonDataConnections { get; set; }

    public virtual DbSet<LoanParticularsInformation> LoanParticularsInformations { get; set; }

    public virtual DbSet<ModeOfPayment> ModeOfPayments { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ModuleStage> ModuleStages { get; set; }

    public virtual DbSet<ModuleStageApprover> ModuleStageApprovers { get; set; }

    public virtual DbSet<ModuleStatus> ModuleStatuses { get; set; }

    public virtual DbSet<ModuleType> ModuleTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationPriorityLevel> NotificationPriorityLevels { get; set; }

    public virtual DbSet<NotificationReceiver> NotificationReceivers { get; set; }

    public virtual DbSet<PropertyType> PropertyTypes { get; set; }

    public virtual DbSet<PurposeOfLoan> PurposeOfLoans { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleAccess> RoleAccesses { get; set; }

    public virtual DbSet<Spouse> Spouses { get; set; }

    public virtual DbSet<SqlDataConnection> SqlDataConnections { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivity> UserActivities { get; set; }

    public virtual DbSet<UserApprover> UserApprovers { get; set; }

    public virtual DbSet<UserDocument> UserDocuments { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC0724120A67");

            entity.ToTable("Address");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDefault).HasDefaultValueSql("((0))");
            entity.Property(e => e.ReferenceType).HasComment("Company = 1, Vendor = 2, Customer = 3, Employee = 3");
            entity.Property(e => e.StreetAddress1).IsRequired();
        });

        modelBuilder.Entity<AddressType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AddressT__3214EC07B5DFABCA");

            entity.ToTable("AddressType");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ApplicantsPersonalInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applican__3214EC074BC4A384");

            entity.ToTable("ApplicantsPersonalInformation");

            entity.Property(e => e.ApprovalStatus).HasDefaultValueSql("((1))");
            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ApprovalLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC07F04351C6");

            entity.ToTable("ApprovalLevel");

            entity.Property(e => e.DateUpdated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Level).HasComment("ApprovalLevel");
        });

        modelBuilder.Entity<ApprovalLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC07FEC2BF26");

            entity.ToTable("ApprovalLog");

            entity.Property(e => e.Action).HasComment("1 = Approved, 2 = Rejected, 3 = Cancelled");
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.ReferenceId).HasComment("Transaction Record Id");
            entity.Property(e => e.StageId).HasComment("Module Stage Id");
        });

        modelBuilder.Entity<ApprovalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC0712B4DE21");

            entity.ToTable("ApprovalStatus");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReferenceId).HasComment("Record Id");
            entity.Property(e => e.ReferenceType).HasComment("Module Id");
            entity.Property(e => e.Status).HasComment("0 = For Approval, 1 = Approved, 2 = Canceled");
            entity.Property(e => e.UserId).HasComment("Prepared By");
        });

        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditTra__3214EC0744688C5C");

            entity.ToTable("AuditTrail");

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ChangeDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ColumnName)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.NewValue)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.OldValue)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.RecordPk)
                .IsRequired()
                .HasMaxLength(4000);
            entity.Property(e => e.TableName)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<BarrowersInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Barrower__3214EC0771E0F56D");

            entity.ToTable("BarrowersInformation");

            entity.Property(e => e.BusinessBaranggayName).HasMaxLength(255);
            entity.Property(e => e.BusinessBuildingName).HasMaxLength(255);
            entity.Property(e => e.BusinessContactNumber).HasMaxLength(20);
            entity.Property(e => e.BusinessCountry).HasMaxLength(255);
            entity.Property(e => e.BusinessEmail).HasMaxLength(255);
            entity.Property(e => e.BusinessLotName).HasMaxLength(255);
            entity.Property(e => e.BusinessMunicipalityName).HasMaxLength(255);
            entity.Property(e => e.BusinessProvinceName).HasMaxLength(255);
            entity.Property(e => e.BusinessStreetName).HasMaxLength(255);
            entity.Property(e => e.BusinessSubdivisionName).HasMaxLength(255);
            entity.Property(e => e.BusinessUnitName).HasMaxLength(255);
            entity.Property(e => e.BusinessZipCode).HasMaxLength(255);
            entity.Property(e => e.Citizenship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployerName).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.HomeNumber).HasMaxLength(50);
            entity.Property(e => e.HomeOwnerShip).HasMaxLength(255);
            entity.Property(e => e.IndustryName).HasMaxLength(100);
            entity.Property(e => e.IsBusinessAddressAbroad).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsPermanentAddressAbroad).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsPresentAddressAbroad).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.MobileNumber).HasMaxLength(50);
            entity.Property(e => e.OcupationStatus).HasMaxLength(100);
            entity.Property(e => e.PermanentBaranggayName).HasMaxLength(255);
            entity.Property(e => e.PermanentBuildingName).HasMaxLength(255);
            entity.Property(e => e.PermanentLotName).HasMaxLength(255);
            entity.Property(e => e.PermanentMunicipalityName).HasMaxLength(255);
            entity.Property(e => e.PermanentProvinceName).HasMaxLength(255);
            entity.Property(e => e.PermanentStreetName).HasMaxLength(255);
            entity.Property(e => e.PermanentSubdivisionName).HasMaxLength(255);
            entity.Property(e => e.PermanentUnitName).HasMaxLength(255);
            entity.Property(e => e.PermanentZipCode).HasMaxLength(255);
            entity.Property(e => e.PositionName).HasMaxLength(100);
            entity.Property(e => e.PreparedMailingAddress).HasMaxLength(255);
            entity.Property(e => e.PresentBaranggayName).HasMaxLength(255);
            entity.Property(e => e.PresentBuildingName).HasMaxLength(255);
            entity.Property(e => e.PresentLotName).HasMaxLength(255);
            entity.Property(e => e.PresentMunicipalityName).HasMaxLength(255);
            entity.Property(e => e.PresentProvinceName).HasMaxLength(255);
            entity.Property(e => e.PresentStreetName).HasMaxLength(255);
            entity.Property(e => e.PresentSubdivisionName).HasMaxLength(255);
            entity.Property(e => e.PresentUnitName).HasMaxLength(255);
            entity.Property(e => e.PresentZipCode).HasMaxLength(255);
            entity.Property(e => e.PropertyDeveloperName).HasMaxLength(255);
            entity.Property(e => e.PropertyLocation).HasMaxLength(255);
            entity.Property(e => e.PropertyUnitLevelName).HasMaxLength(255);
            entity.Property(e => e.SSSNumber).HasMaxLength(150);
            entity.Property(e => e.Sex).HasMaxLength(20);
            entity.Property(e => e.Suffix).HasMaxLength(50);
            entity.Property(e => e.TinNumber).HasMaxLength(150);
        });

        modelBuilder.Entity<CollateralInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Collater__3214EC07150A135D");

            entity.ToTable("CollateralInformation");

            entity.Property(e => e.CollateralReason).IsRequired();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeveloperName)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.ExistingTotalFloorArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LandArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Municipality)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.ProposedTotalFloorArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Province)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(e => e.Street)
                .IsRequired()
                .HasMaxLength(250);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Company__3214EC07960DD044");

            entity.ToTable("Company", tb => tb.HasTrigger("Trigger_Company_ColumnUpdates"));

            entity.Property(e => e.AccountingPeriod).HasMaxLength(25);
            entity.Property(e => e.AcctngPeriodFrom).HasMaxLength(50);
            entity.Property(e => e.AcctngPeriodTo).HasMaxLength(50);
            entity.Property(e => e.BusinessStyle).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FaxNo).HasMaxLength(50);
            entity.Property(e => e.IsDisabled).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsRequiredDailySetCurrency).HasDefaultValueSql("((0))");
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.RepresentativeDesignation).HasMaxLength(100);
            entity.Property(e => e.RepresentativeName).HasMaxLength(255);
            entity.Property(e => e.RepresentativeTin).HasMaxLength(50);
            entity.Property(e => e.TelNo).HasMaxLength(50);
            entity.Property(e => e.Tin).HasMaxLength(50);
            entity.Property(e => e.Website).HasMaxLength(50);
        });

        modelBuilder.Entity<CompanyLogo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyL__3214EC0781D59D9C");

            entity.ToTable("CompanyLogo");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.CompanyLogos)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_CompanyLogo_Company");
        });

        modelBuilder.Entity<CompanySetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CompanyS__3214EC071F8345BC");

            entity.ToTable("CompanySetting");

            entity.Property(e => e.AccountingPeriod).HasMaxLength(25);
            entity.Property(e => e.AcctgPeriodFrom).HasMaxLength(50);
            entity.Property(e => e.AcctgPeriodTo).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country", tb => tb.HasTrigger("Trigger_Country_ColumnUpdates"));

            entity.Property(e => e.Capital).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Continent).HasMaxLength(50);
            entity.Property(e => e.ContinentName).HasMaxLength(100);
            entity.Property(e => e.CurrencyCode).HasMaxLength(50);
            entity.Property(e => e.East).HasMaxLength(50);
            entity.Property(e => e.FipsCode).HasMaxLength(50);
            entity.Property(e => e.IsoAlpha3).HasMaxLength(50);
            entity.Property(e => e.IsoNumber).HasMaxLength(50);
            entity.Property(e => e.Languages).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.North).HasMaxLength(50);
            entity.Property(e => e.South).HasMaxLength(50);
            entity.Property(e => e.West).HasMaxLength(50);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC070EEF1A71");

            entity.ToTable("Document");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ReferenceNo)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07A6594FAD");

            entity.ToTable("DocumentType");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Form2Page>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Form2Pag__3214EC0712B1369D");

            entity.ToTable("Form2Page");

            entity.Property(e => e.Agreement).HasDefaultValueSql("((1))");
            entity.Property(e => e.Amortization1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amortization2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Amortization3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBalance1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBalance2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountBalance3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AquisitionCost1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AquisitionCost2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AquisitionCost3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AverageBalance1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AverageBalance2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AverageBalance3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bank1).HasMaxLength(255);
            entity.Property(e => e.Bank2).HasMaxLength(255);
            entity.Property(e => e.Bank3).HasMaxLength(255);
            entity.Property(e => e.BankFinancial1).HasMaxLength(255);
            entity.Property(e => e.BankFinancial2).HasMaxLength(255);
            entity.Property(e => e.BankFinancial3).HasMaxLength(255);
            entity.Property(e => e.BranchAddress1).HasMaxLength(255);
            entity.Property(e => e.BranchAddress2).HasMaxLength(255);
            entity.Property(e => e.BranchAddress3).HasMaxLength(255);
            entity.Property(e => e.CardType1).HasMaxLength(255);
            entity.Property(e => e.CardType2).HasMaxLength(255);
            entity.Property(e => e.CardType3).HasMaxLength(255);
            entity.Property(e => e.CharacterAddress1).HasMaxLength(255);
            entity.Property(e => e.CharacterAddress2).HasMaxLength(255);
            entity.Property(e => e.CharacterAddress3).HasMaxLength(255);
            entity.Property(e => e.CharacterNameSupplier1).HasMaxLength(255);
            entity.Property(e => e.CharacterNameSupplier2).HasMaxLength(255);
            entity.Property(e => e.CharacterNameSupplier3).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.CreditLimit1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditLimit2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditLimit3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditorAndAddress1).HasMaxLength(255);
            entity.Property(e => e.CreditorAndAddress2).HasMaxLength(255);
            entity.Property(e => e.CreditorAndAddress3).HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(355);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.HighestAmount1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HighestAmount2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HighestAmount3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IssuerName1).HasMaxLength(255);
            entity.Property(e => e.IssuerName2).HasMaxLength(255);
            entity.Property(e => e.IssuerName3).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.LoanSecurity1).HasMaxLength(255);
            entity.Property(e => e.LoanSecurity2).HasMaxLength(255);
            entity.Property(e => e.LoanSecurity3).HasMaxLength(255);
            entity.Property(e => e.Location1).HasMaxLength(255);
            entity.Property(e => e.Location2).HasMaxLength(255);
            entity.Property(e => e.Location3).HasMaxLength(255);
            entity.Property(e => e.MIddleName).HasMaxLength(255);
            entity.Property(e => e.MarketValue1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MarketValue2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MarketValue3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MortgageBalance1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MortgageBalance2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MortgageBalance3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NameSupplier1).HasMaxLength(255);
            entity.Property(e => e.NameSupplier2).HasMaxLength(255);
            entity.Property(e => e.NameSupplier3).HasMaxLength(255);
            entity.Property(e => e.PresentBalance1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PresentBalance2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PresentBalance3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Purpose1).HasMaxLength(255);
            entity.Property(e => e.Purpose2).HasMaxLength(255);
            entity.Property(e => e.Purpose3).HasMaxLength(255);
            entity.Property(e => e.RentalIncome1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RentalIncome2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RentalIncome3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Security1).HasMaxLength(255);
            entity.Property(e => e.Security2).HasMaxLength(255);
            entity.Property(e => e.Security3).HasMaxLength(255);
            entity.Property(e => e.SellersBaranggayName).HasMaxLength(255);
            entity.Property(e => e.SellersBuildingName).HasMaxLength(255);
            entity.Property(e => e.SellersLotName).HasMaxLength(255);
            entity.Property(e => e.SellersMunicipalityName).HasMaxLength(255);
            entity.Property(e => e.SellersProvinceName).HasMaxLength(255);
            entity.Property(e => e.SellersStreetName).HasMaxLength(255);
            entity.Property(e => e.SellersSubdivisionName).HasMaxLength(255);
            entity.Property(e => e.SellersUnitName).HasMaxLength(255);
            entity.Property(e => e.SellersZipCode).HasMaxLength(255);
            entity.Property(e => e.Suffix).HasMaxLength(255);
            entity.Property(e => e.TinNumber).HasMaxLength(150);
            entity.Property(e => e.TradeAddress1).HasMaxLength(255);
            entity.Property(e => e.TradeAddress2).HasMaxLength(255);
            entity.Property(e => e.TradeAddress3).HasMaxLength(255);
            entity.Property(e => e.Type1).HasMaxLength(255);
            entity.Property(e => e.Type2).HasMaxLength(255);
            entity.Property(e => e.Type3).HasMaxLength(255);
            entity.Property(e => e.TypeOfAccount1).HasMaxLength(255);
            entity.Property(e => e.TypeOfAccount2).HasMaxLength(255);
            entity.Property(e => e.TypeOfAccount3).HasMaxLength(255);
            entity.Property(e => e.TypeOfProperty1).HasMaxLength(255);
            entity.Property(e => e.TypeOfProperty2).HasMaxLength(255);
            entity.Property(e => e.TypeOfProperty3).HasMaxLength(255);
        });

        modelBuilder.Entity<JsonDataConnection>(entity =>
        {
            entity.Property(e => e.ConnectionString).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<LoanParticularsInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoanPart__3214EC070F9494C8");

            entity.ToTable("LoanParticularsInformation");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DesiredLoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExistingHousingApplicationNumber).HasMaxLength(255);
        });

        modelBuilder.Entity<ModeOfPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ModeOfPa__3214EC079FA8113D");

            entity.ToTable("ModeOfPayment");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Module__3214EC078BECABC4");

            entity.ToTable("Module", tb => tb.HasTrigger("Trigger_Module_ColumnUpdates"));

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ModuleStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ModuleSt__3214EC07303A1660");

            entity.ToTable("ModuleStage", tb => tb.HasTrigger("Trigger_ModuleStage_ColumnUpdates"));

            entity.Property(e => e.ApproveDesc)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.RejectDesc)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<ModuleStageApprover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07C1F9F105");

            entity.ToTable("ModuleStageApprover");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ModuleStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ModuleSt__3214EC072C522C3B");

            entity.ToTable("ModuleStatus");

            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<ModuleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ModuleTy__3214EC0735E796BC");

            entity.ToTable("ModuleType", tb => tb.HasTrigger("Trigger_ModuleType_ColumnUpdates"));

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.IsDisabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.IsVisible)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC071A7E47EA");

            entity.ToTable("Notification");

            entity.Property(e => e.ActionLink)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Preview)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<NotificationPriorityLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07C3B5075B");

            entity.ToTable("NotificationPriorityLevel");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.LevelName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<NotificationReceiver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC077C3F1AEF");

            entity.ToTable("NotificationReceiver");

            entity.Property(e => e.DateRead).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Notif).WithMany(p => p.NotificationReceivers)
                .HasForeignKey(d => d.NotifId)
                .HasConstraintName("FK__Notificat__Notif__367C1819");
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Property__3214EC079EAF53BC");

            entity.ToTable("PropertyType");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<PurposeOfLoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PurposeO__3214EC070CA1E0E7");

            entity.ToTable("PurposeOfLoan");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.Property(e => e.DisplayName).IsRequired();
            entity.Property(e => e.LayoutData).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07B51459A4");

            entity.ToTable("Role");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<RoleAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleAcce__3214EC07AB4EBC05");

            entity.ToTable("RoleAccess");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Spouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Spouse__3214EC07858BACCD");

            entity.ToTable("Spouse");

            entity.Property(e => e.Citizenship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.IsSpouseAddressAbroad).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PreparedMailingAddress).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentBaranggayName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentBuildingName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentLotName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentMunicipalityName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentProvinceName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentStreetName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentSubdivisionName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentUnitName).HasMaxLength(255);
            entity.Property(e => e.SpouseEmploymentZipCode).HasMaxLength(255);
            entity.Property(e => e.Suffix).HasMaxLength(50);
            entity.Property(e => e.TinNumber).HasMaxLength(150);
        });

        modelBuilder.Entity<SqlDataConnection>(entity =>
        {
            entity.Property(e => e.ConnectionString).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07DF45F7BE");

            entity.ToTable("User");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsDark).HasDefaultValueSql("((0))");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Prefix).HasMaxLength(50);
            entity.Property(e => e.ProfilePicture).HasMaxLength(255);
            entity.Property(e => e.RefreshToken).HasMaxLength(255);
            entity.Property(e => e.Signature).HasMaxLength(255);
            entity.Property(e => e.Suffix).HasMaxLength(50);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserActi__3214EC079D4DD9D0");

            entity.ToTable("UserActivity");

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Browser)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Device)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<UserApprover>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserAppr__3214EC07FA23AE67");

            entity.ToTable("UserApprover");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserDocu__3214EC07E7E62F10");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC073AF73B8A");

            entity.ToTable("UserRole");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserToke__3214EC072ABECB8D");

            entity.ToTable("UserToken");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RefreshToken)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(4000);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}