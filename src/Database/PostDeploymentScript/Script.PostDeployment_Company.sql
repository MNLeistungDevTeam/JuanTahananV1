IF NOT EXISTS (SELECT 1 FROM [dbo].[Company])
BEGIN
    SET IDENTITY_INSERT [dbo].[Company] ON
    
    INSERT INTO [dbo].[Company] ([Id], [Code], [Name], [BusinessStyle], [TelNo], [MobileNo], [FaxNo], [Email], [Website], [CreatedById], [DateCreated], [ModifiedById], [DateModified]) 
    VALUES (1, N'JTH-PH', N'Juan Tahanan Ph', N'Housing Platforms', N'8123-4567', N'(091) 234-5678', N'(812) 345-67__', N'info@mnleistung.de', N'https://juantahanan-dms.mnleistung.ph/', 0, GETDATE(), NULL, GETDATE());

    INSERT INTO [dbo].[Company] ([Id], [Code], [Name], [BusinessStyle], [TelNo], [MobileNo], [FaxNo], [Email], [Website], [CreatedById], [DateCreated], [ModifiedById], [DateModified]) 
    VALUES (2, N'RS-PH', N'RS Realty Concepts Developer Inc', N'Real State', N'+028-539-8670', N'+63 917 702 9177', N'(812) 345-67__', N'info@rsrealty.ph', N'https://rsrealty.ph/', 0, GETDATE(), NULL, GETDATE());

    INSERT INTO [dbo].[Company] ([Id], [Code], [Name], [BusinessStyle], [TelNo], [MobileNo], [FaxNo], [Email], [Website], [CreatedById], [DateCreated], [ModifiedById], [DateModified]) 
    VALUES (3, N'ZETA-PH', N'Zeta World Realty Inc', N'Real State', N'8123-4567', N'(+63)91 7453-4927', N'(812) 345-67__', N'info@zeta-alpa-test.zetaworld.ph', N'https://zeta-alpa-test.zetaworld.ph/', 0, GETDATE(), NULL, GETDATE());
    
    SET IDENTITY_INSERT [dbo].[Company] OFF
END
