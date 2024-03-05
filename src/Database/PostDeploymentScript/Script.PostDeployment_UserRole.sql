IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRole])
BEGIN
SET IDENTITY_INSERT [dbo].[UserRole] ON 
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (2, 1, 1)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (3, 2, 4)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (5, 3, 2)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (6, 4, 3)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
END
GO