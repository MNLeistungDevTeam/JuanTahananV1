IF NOT EXISTS (SELECT 1 FROM [dbo].[Company])
BEGIN
    SET IDENTITY_INSERT [dbo].[Company] ON
    
    INSERT INTO [dbo].[Company] ([Id], [Code], [Name], [BusinessStyle], [TelNo], [MobileNo], [FaxNo], [Email], [Website], [CreatedById], [DateCreated], [ModifiedById], [DateModified]) 
    VALUES (1, N'JTH-PH', N'Juan Tahanan Ph', N'Juan Tahanan', N'8123-4567', N'(091) 234-5678', N'(812) 345-67__', N'info@mnleistung.de', N'https://juantahanan-dms.mnleistung.ph/', 0, GETDATE(), NULL, GETDATE())
    
    SET IDENTITY_INSERT [dbo].[Company] OFF
END
