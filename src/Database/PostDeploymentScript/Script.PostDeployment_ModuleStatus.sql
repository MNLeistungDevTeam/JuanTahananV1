IF NOT EXISTS (SELECT 1 FROM [dbo].[ModuleStatus])
BEGIN
SET IDENTITY_INSERT [dbo].[ModuleStatus] ON 
INSERT [dbo].[ModuleStatus] ([Id], [Description], [Color], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (1, N'Maintenance', N'danger', CAST(N'2024-02-20T12:38:34.2600000' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[ModuleStatus] ([Id], [Description], [Color], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (2, N'InActive', N'warning', CAST(N'2024-02-20T12:38:34.2600000' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[ModuleStatus] ([Id], [Description], [Color], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (3, N'Active', N'success', CAST(N'2024-02-20T12:38:34.2600000' AS DateTime2), 1, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ModuleStatus] OFF
END
GO