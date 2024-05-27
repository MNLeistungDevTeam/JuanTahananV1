 IF NOT EXISTS (SELECT 1 FROM [dbo].[PropertyProject])
BEGIN
SET IDENTITY_INSERT [dbo].[PropertyProject] ON 
INSERT [dbo].[PropertyProject] ([Id],[Name], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById]) VALUES (1, N'Arao Yuhum Residences', N'a project house for filipino', CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL,NULL)
 
 SET IDENTITY_INSERT [dbo].[PropertyProject] OFF
END
GO

 IF NOT EXISTS (SELECT 1 FROM [dbo].[PropertyLocation])
BEGIN
SET IDENTITY_INSERT [dbo].[PropertyLocation] ON 
INSERT [dbo].[PropertyLocation] ([Id],[Name],DateCreated,CreatedById,DateModified,ModifiedById) VALUES (1, N'Bacolod Rizal', CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL,NULL)
 
 SET IDENTITY_INSERT [dbo].[PropertyLocation] OFF
END
GO

   IF NOT EXISTS (SELECT 1 FROM [dbo].[PropertyUnit])
BEGIN
SET IDENTITY_INSERT [dbo].[PropertyUnit] ON 
INSERT [dbo].[PropertyUnit] ([Id],[Name],[Description],DateCreated,CreatedById,DateModified,ModifiedById) VALUES (1, N'Single House', N'A 24 sqm With 4 Bed Room',CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL,NULL)
 
 SET IDENTITY_INSERT [dbo].[PropertyUnit] OFF
END
GO
