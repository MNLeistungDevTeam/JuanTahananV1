 IF NOT EXISTS (SELECT 1 FROM [dbo].[EmailSetup])
BEGIN
SET IDENTITY_INSERT [dbo].[EmailSetup] ON 
INSERT [dbo].[EmailSetup] ([Id], Email, [Password], Host, DisplayName, [Port], CompanyId,DateCreated,CreatedById,IsDefault) VALUES (1, N'emailserverbootptc@gmail.com', N'hdmgfgnxlkaneria', N'smtp.gmail.com', N'eiDoc',587,1, CAST(N'2024-02-22T08:40:10.7014438' AS DateTime2), 1,1)
 SET IDENTITY_INSERT [dbo].[EmailSetup] OFF
END
GO