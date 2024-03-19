IF NOT EXISTS (SELECT 1 FROM [dbo].[PurposeOfLoan])
BEGIN
SET IDENTITY_INSERT [dbo].[PurposeOfLoan] ON 
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (1, N'Purchase of fully developed residential lot or adjoining residential lots', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (2, N'Purchase of a residential house and lot, townhouse or condominium unit, inclusive of a parking slot ', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (3, N'Construction or completion of a residential unit on a residential lot', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (4, N'Home improvement', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (5, N'Refinancing of an existing housing loan', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (6, N'Purchase of residential lot plus cost of transfer of title', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (7, N'Purchase of residential unit plus cost of transfer of title', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
INSERT [dbo].[PurposeOfLoan] ([Id], [Description], [DateCreated], [CreatedById]) VALUES (8, N'Purchase of a parking slot', CAST(N'2024-02-25T11:38:44.2666667' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[PurposeOfLoan] OFF
END
GO