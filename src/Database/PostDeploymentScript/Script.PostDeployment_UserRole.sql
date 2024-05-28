IF NOT EXISTS (SELECT 1 FROM [dbo].[UserRole])
BEGIN
SET IDENTITY_INSERT [dbo].[UserRole] ON 
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (1, 1, 1)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (2, 2, 2)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (3, 3, 3)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (4, 4, 4)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (5, 5, 5)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (6, 6, 5)

INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (7, 7, 1)
INSERT [dbo].[UserRole] ([Id], [UserId], [RoleId]) VALUES (8, 8, 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
END
GO