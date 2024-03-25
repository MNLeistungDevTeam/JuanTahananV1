﻿IF NOT EXISTS (SELECT 1 FROM [dbo].[DocumentType])
BEGIN
SET IDENTITY_INSERT [dbo].[DocumentType] ON 
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (3, N'PAGIBIG BUYERS CONFIRMATION', CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (4, N'PROOF OF INCOME', CAST(N'2024-02-24T08:31:57.0094831' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (5, N'PAGIBIG HOUSING LOAN APPLICATION WITH RECENT VALID ID OF BORROWER & CO-BORROWER', CAST(N'2024-02-24T08:32:05.9640793' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (6, N'BIRTH CERTIFICATE', CAST(N'2024-02-24T08:32:12.9360798' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (7, N'MARRIAGE CERTIFICATE OR CENOMAR', CAST(N'2024-02-24T08:32:21.6886051' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (8, N'CEDULA', CAST(N'2024-02-24T08:32:32.3003803' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (9, N'1 X 1 ID PICTURE', CAST(N'2024-02-24T08:32:44.9771157' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (10, N'TCT (TRANSFER CERTIFICATE OF TITLE)', CAST(N'2024-02-24T08:32:59.0298253' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (11, N'CCT (CONDOMINIUM CERTIFICATE OF TITLE)', CAST(N'2024-02-24T08:33:06.3514489' AS DateTime2), 1, CAST(N'2024-02-24T08:33:46.5876187' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (12, N'HLF069 HOUSING LOAN APPLICATION COBORROWER', CAST(N'2024-02-24T08:33:20.1570733' AS DateTime2), 1, CAST(N'2024-02-24T08:33:56.1223448' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (13, N'HLF069 HOUSING LOAN APPLICATION COBORROWER', CAST(N'2024-02-24T08:33:39.5042810' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (14, N'HLF058 BORROWERS VALIDATION SHEET DEVELOPER ASSISTED', CAST(N'2024-02-24T08:34:16.8148136' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (15, N'HLF062 DEVELOPER SWORN CERTIFICATION', CAST(N'2024-02-24T08:34:26.8207202' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (16, N'HLF064 SPECIAL POWER OF ATTORNEY', CAST(N'2024-02-24T08:34:38.5568817' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (17, N'HLF083 CERTIFICATE OF ACCEPTANCE', CAST(N'2024-02-24T08:34:46.8078092' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (18, N'HLF086 PROMISSORY NOTE HOME FINANCING PROGRAM', CAST(N'2024-02-24T08:34:53.7658978' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (19, N'HLF087 PROMISSORY NOTE AFFORDABLE HOUSING PROGRAM', CAST(N'2024-02-24T08:35:00.7742975' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (20, N'HLF234 DCS ENDUSER PROGRAM', CAST(N'2024-02-24T08:35:20.8384282' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (21, N'HLF235 DCS AFFORDABLE HOUSING', CAST(N'2024-02-24T08:35:28.3170334' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (22, N'HLF236 DOAS REGULAR HOUSING', CAST(N'2024-02-24T08:35:38.3197435' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (23, N'WLF182 CHECKLIST REQUIREMENTS PRELIMINARY CONDOMINIUM APPRAISAL', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)


INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (24, N'HLF1035 Housing Loan Application (V01)', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (25, N'HLF1036 Housing Loan Application Co-borrower ', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (26, N'HLF1042 Borrower-Beneficiary Conformity', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (27, N'HLF 1046 Authority to Deduct 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (28, N'HLF1069 Conformity Non-relatives 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id], [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (29, N'WLF252 Buyer Confirmation Form', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL);
SET IDENTITY_INSERT [dbo].[DocumentType] OFF
END
GO


IF NOT EXISTS (SELECT 1 FROM [dbo].DocumentVerification)
BEGIN
	SET IDENTITY_INSERT [dbo].DocumentVerification ON
 -- Insert dummy data with document type ids 5, 3, 4, 6, 7, 8, and 9
INSERT INTO [dbo].[DocumentVerification] ([Id], DocumentTypeId, [Type], CreatedById, DateCreated, ModifiedById, DateModified)
VALUES 
    (1, 5, 1, 1, GETDATE(), NULL, NULL), 
    (2, 3, 1, 1, GETDATE(), NULL, NULL), 
    (3, 4, 1, 1, GETDATE(), NULL, NULL), 
    (4, 6, 1, 1, GETDATE(), NULL, NULL), 
    (5, 7, 1, 1, GETDATE(), NULL, NULL), 
    (6, 8, 1, 1, GETDATE(), NULL, NULL), 
    (7, 9, 1, 1, GETDATE(), NULL, NULL),
    (8, 24, 2, 1, GETDATE(), NULL, NULL), 
    (9, 25, 2, 1, GETDATE(), NULL, NULL), 
    (10, 26, 2, 1, GETDATE(), NULL, NULL),
    (11, 27, 2, 1, GETDATE(), NULL, NULL), 
    (12, 28, 2, 1, GETDATE(), NULL, NULL), 
    (13, 29, 2, 1, GETDATE(), NULL, NULL);

 
SET IDENTITY_INSERT [dbo].DocumentVerification OFF

END
GO