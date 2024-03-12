CREATE TABLE [dbo].[Address] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ReferenceId]      INT            NOT NULL,
    [ReferenceType]    INT            NOT NULL,
    [StreetAddress1]   NVARCHAR (MAX) NOT NULL,
    [StreetAddress2]   NVARCHAR (MAX) NULL,
    [Baranggay]        NVARCHAR (MAX)  NULL,
    [CityMunicipality] NVARCHAR (MAX)  NULL,
    [Municipality]     NVARCHAR (MAX)  NULL,
    [Province]         NVARCHAR (MAX)  NULL,
    [StateProvince]    NVARCHAR (MAX)  NULL,
    [Region]           NVARCHAR (MAX)  NULL,
    [PostalCode]       INT            NOT NULL,
    [CountryId]        INT            NULL,
    [Remarks]          NVARCHAR (MAX) NULL,
    [IsDisabled]       BIT            DEFAULT 0 NOT NULL,
    [CreatedById]      INT            NOT NULL,
    [DateCreated]      DATETIME2 (7)  DEFAULT GETDATE() NOT NULL,
    [ModifiedById]     INT            NOT NULL,
    [DateModified]     DATETIME2 (7)  NOT NULL,
    [IsDefault]        BIT            DEFAULT 0 NULL,
    [AddressName]      NVARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    --CONSTRAINT [FK_Address_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id])
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Company = 1, Vendor = 2, Customer = 3, Employee = 3',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Address',
    @level2type = N'COLUMN',
    @level2name = N'ReferenceType'