IF NOT EXISTS (SELECT 1 FROM [dbo].[Role])
BEGIN
SET IDENTITY_INSERT [dbo].[Role] ON 
INSERT [dbo].[Role] ([Id], [Name], [Description], [IsDisabled], [IsLocked], [DateCreated], [DateModified]) VALUES (1, N'Admin', N'Super Admin', 0, 1, CAST(N'2024-02-22T08:40:10.7014438' AS DateTime2), NULL)
INSERT [dbo].[Role] ([Id], [Name], [Description], [IsDisabled], [IsLocked], [DateCreated], [DateModified]) VALUES (2, N'LGU', N'Local Government Unit (LGU)', 0, 1, CAST(N'2024-02-22T08:40:48.9875591' AS DateTime2), NULL)
INSERT [dbo].[Role] ([Id], [Name], [Description], [IsDisabled], [IsLocked], [DateCreated], [DateModified]) VALUES (3, N'Pagibig', N'Pag-ibig', 0, 1, CAST(N'2024-02-22T08:41:14.3333617' AS DateTime2), NULL)
INSERT [dbo].[Role] ([Id], [Name], [Description], [IsDisabled], [IsLocked], [DateCreated], [DateModified]) VALUES (4, N'Beneficiary', N'Beneficiary', 0, 1, CAST(N'2024-02-22T08:41:35.7505678' AS DateTime2), NULL)
INSERT [dbo].[Role] ([Id], [Name], [Description], [IsDisabled], [IsLocked], [DateCreated], [DateModified]) VALUES (5, N'Developer', N'Developer', 0, 1, CAST(N'2024-02-22T08:41:35.7505678' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF
END
GO