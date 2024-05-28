 
 IF NOT EXISTS (SELECT 1 FROM [dbo].[UserCompany])
BEGIN
    -- Enable IDENTITY_INSERT
    SET IDENTITY_INSERT [dbo].[UserCompany] ON;

    -- Insert statements
    INSERT INTO [dbo].[UserCompany] (
        [Id],
       UserId,
       CompanyId,
        [CreatedById],
        [DateCreated],
        ModifiedById,
        [DateModified]
    ) 
    VALUES 
        (1, 5,2, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, NULL),
        (2, 6,3, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, NULL),
        (3, 7,2, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, NULL),
        (4, 8,3, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, NULL);
     
    -- Disable IDENTITY_INSERT
    SET IDENTITY_INSERT [dbo].[UserCompany] OFF;
END;
GO