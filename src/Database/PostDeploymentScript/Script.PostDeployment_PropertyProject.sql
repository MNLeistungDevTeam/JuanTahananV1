IF NOT EXISTS (SELECT 1 FROM [dbo].[PropertyProject])
BEGIN
    SET IDENTITY_INSERT [dbo].[PropertyProject] ON

    INSERT INTO [dbo].[PropertyProject]
           ([Name]
           ,[Description]
           ,[Logo]
           ,[CompanyId]
           ,[CreatedById])
    VALUES
        (1, N'Arao Yuhum Residences', N'a project house for filipino', N'', 2, 1, GETDATE())

    SET IDENTITY_INSERT [dbo].[PropertyProject] OFF
END

