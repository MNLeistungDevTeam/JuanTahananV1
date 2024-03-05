﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using Template.Infrastructure;

namespace Template.Infrastructure.Persistence;

public partial class MNLTemplateDBContext : DbContext
{
    public MNLTemplateDBContext(DbContextOptions<MNLTemplateDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicantsPersonalInformation> ApplicantsPersonalInformations { get; set; }

    public virtual DbSet<AuditTrail> AuditTrails { get; set; }

    public virtual DbSet<BarrowersInformation> BarrowersInformations { get; set; }

    public virtual DbSet<CollateralInformation> CollateralInformations { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Form2Page> Form2Pages { get; set; }

    public virtual DbSet<JsonDataConnection> JsonDataConnections { get; set; }

    public virtual DbSet<LoanParticularsInformation> LoanParticularsInformations { get; set; }

    public virtual DbSet<ModeOfPayment> ModeOfPayments { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<ModuleStatus> ModuleStatuses { get; set; }

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
        modelBuilder.HasAnnotation("Scaffolding:ConnectionString", "Data Source=(local);Initial Catalog=Template.Database;Integrated Security=true");

        modelBuilder.Entity<ApplicantsPersonalInformation>(entity =>
        {
            entity.ToTable("ApplicantsPersonalInformation");

            entity.Property(e => e.Code).IsRequired();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<AuditTrail>(entity =>
        {
            entity.ToTable("AuditTrail");

            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ChangeDate).HasDefaultValueSql("getdate()");
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
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.DepartmentName).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployerName).HasMaxLength(100);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.HomeOwnerShip).HasMaxLength(255);
            entity.Property(e => e.IndustryName).HasMaxLength(100);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
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
            entity.Property(e => e.Sex).HasMaxLength(20);
            entity.Property(e => e.Suffix).HasMaxLength(50);
        });

        modelBuilder.Entity<CollateralInformation>(entity =>
        {
            entity.ToTable("CollateralInformation");

            entity.Property(e => e.CollateralReason).IsRequired();
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.DeveloperName).IsRequired();
            entity.Property(e => e.ExistingTotalFloorArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LandArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Municipality).IsRequired();
            entity.Property(e => e.ProposedTotalFloorArea).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Province).IsRequired();
            entity.Property(e => e.Street).IsRequired();
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Document");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.DateModified).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Guid).HasDefaultValueSql("NEWID()");
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
            entity.ToTable("DocumentType");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Form2Page>(entity =>
        {
            entity.ToTable("Form2Page");

            entity.Property(e => e.Agreement)
                .IsRequired()
                .HasDefaultValueSql("1");
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
            entity.Property(e => e.CreditLimit1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditLimit2).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditLimit3).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreditorAndAddress1).HasMaxLength(255);
            entity.Property(e => e.CreditorAndAddress2).HasMaxLength(255);
            entity.Property(e => e.CreditorAndAddress3).HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
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
            entity.ToTable("LoanParticularsInformation");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.DesiredLoanAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExistingHousingApplicationNumber)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<ModeOfPayment>(entity =>
        {
            entity.ToTable("ModeOfPayment");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.ToTable("Module");

            entity.Property(e => e.Action).HasMaxLength(255);
            entity.Property(e => e.BreadName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Controller).HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Icon)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ModuleStatusId).HasDefaultValueSql("1");
        });

        modelBuilder.Entity<ModuleStatus>(entity =>
        {
            entity.ToTable("ModuleStatus");

            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            entity.ToTable("PropertyType");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Description).IsRequired();
        });

        modelBuilder.Entity<PurposeOfLoan>(entity =>
        {
            entity.ToTable("PurposeOfLoan");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
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
            entity.ToTable("Role");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("getdate()");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<RoleAccess>(entity =>
        {
            entity.ToTable("RoleAccess");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("getdate()");
            entity.Property(e => e.DateModified).HasDefaultValueSql("getdate()");
        });

        modelBuilder.Entity<Spouse>(entity =>
        {
            entity.ToTable("Spouse");

            entity.Property(e => e.Citizenship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
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
        });

        modelBuilder.Entity<SqlDataConnection>(entity =>
        {
            entity.Property(e => e.ConnectionString).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired();
            entity.Property(e => e.Name).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsDark).HasDefaultValueSql("0");
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
            entity.ToTable("UserApprover");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("getdate()");
        });

        modelBuilder.Entity<UserDocument>(entity =>
        {
            entity.Property(e => e.DateCreated).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRole");
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("UserToken");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("getdate()");
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