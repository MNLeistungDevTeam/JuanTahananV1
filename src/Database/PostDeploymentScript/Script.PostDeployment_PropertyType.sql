IF NOT EXISTS (SELECT 1 FROM [dbo].[PropertyType])
BEGIN
SET IDENTITY_INSERT [dbo].[PropertyType] ON 
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (1, N'Rowhouse', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (2, N'Single Attached', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (3, N'Single Detached', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (4, N'Condominium', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (5, N'Townhouse', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
INSERT [dbo].[PropertyType] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (6, N'Duplex', CAST(N'2024-02-25T19:42:43.6700000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[PropertyType] OFF
END
GO