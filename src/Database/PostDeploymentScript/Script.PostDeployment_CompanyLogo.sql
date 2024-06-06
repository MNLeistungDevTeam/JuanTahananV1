IF NOT EXISTS (SELECT 1 FROM [dbo].[CompanyLogo])
BEGIN
    SET IDENTITY_INSERT [dbo].[CompanyLogo] ON

    INSERT INTO [dbo].[CompanyLogo] ([Id], [Description], [Location], [IsDisabled], [CreatedById], [DateCreated], [ModifiedById], [DateModified], [CompanyId])
    VALUES 
        (1, N'ReportLogo', N'/Files/Images/CompanyLogoFile/RS Realty Concepts Developer Inc/dDPZc5y1QG.png', 0, 1, '2024-06-04 18:09:45.6009619', NULL, NULL, 2),
        (2, N'ReportLogo', N'/Files/Images/CompanyLogoFile/Zeta World Realty Inc/GXWT1LQfSI.png', 0, 1, '2024-06-04 18:09:45.6009619', NULL, NULL, 3)

    SET IDENTITY_INSERT [dbo].[CompanyLogo] OFF
END


