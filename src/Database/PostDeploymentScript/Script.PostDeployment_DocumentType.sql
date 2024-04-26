IF NOT EXISTS (SELECT 1 FROM [dbo].[DocumentType])
BEGIN
SET IDENTITY_INSERT [dbo].[DocumentType] ON 
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (1, N'PAGIBIG-BCNFIRM', N'PAGIBIG BUYERS CONFIRMATION', CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (2,  N'PROOF-INCM',N'PROOF OF INCOME', CAST(N'2024-02-24T08:31:57.0094831' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (3, N'PAGIBIG-HLAW/RCNTVALID-ID', N'PAGIBIG HOUSING LOAN APPLICATION WITH RECENT VALID ID OF BORROWER & CO-BORROWER', CAST(N'2024-02-24T08:32:05.9640793' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (4, N'BC', N'BIRTH CERTIFICATE', CAST(N'2024-02-24T08:32:12.9360798' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (5, N'MC', N'MARRIAGE CERTIFICATE OR CENOMAR', CAST(N'2024-02-24T08:32:21.6886051' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (6, N'CD', N'CEDULA', CAST(N'2024-02-24T08:32:32.3003803' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (7, N'ID', N'1 X 1 ID PICTURE', CAST(N'2024-02-24T08:32:44.9771157' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (8, N'TCT', N'TCT (TRANSFER CERTIFICATE OF TITLE)', CAST(N'2024-02-24T08:32:59.0298253' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (9, N'CCT', N'CCT (CONDOMINIUM CERTIFICATE OF TITLE)', CAST(N'2024-02-24T08:33:06.3514489' AS DateTime2), 1, CAST(N'2024-02-24T08:33:46.5876187' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (10, N'HLA-CB', N'HLF069 HOUSING LOAN APPLICATION COBORROWER', CAST(N'2024-02-24T08:33:20.1570733' AS DateTime2), 1, CAST(N'2024-02-24T08:33:56.1223448' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (11, N'BVS-DA', N'HLF058 BORROWERS VALIDATION SHEET DEVELOPER ASSISTED', CAST(N'2024-02-24T08:34:16.8148136' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (12, N'DSC', N'HLF062 DEVELOPER SWORN CERTIFICATION', CAST(N'2024-02-24T08:34:26.8207202' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (13, N'SPA', N'HLF064 SPECIAL POWER OF ATTORNEY', CAST(N'2024-02-24T08:34:38.5568817' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (14, N'CA', N'HLF083 CERTIFICATE OF ACCEPTANCE', CAST(N'2024-02-24T08:34:46.8078092' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (15, N'PN-HF', N'HLF086 PROMISSORY NOTE HOME FINANCING PROGRAM', CAST(N'2024-02-24T08:34:53.7658978' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (16, N'PN-AH', N'HLF087 PROMISSORY NOTE AFFORDABLE HOUSING PROGRAM', CAST(N'2024-02-24T08:35:00.7742975' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (17, N'DCS-EP', N'HLF234 DCS ENDUSER PROGRAM', CAST(N'2024-02-24T08:35:20.8384282' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (18, N'DCS-AH', N'HLF235 DCS AFFORDABLE HOUSING', CAST(N'2024-02-24T08:35:28.3170334' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (19, N'DOAS-RH', N'HLF236 DOAS REGULAR HOUSING', CAST(N'2024-02-24T08:35:38.3197435' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (20, N'CR-PCA', N'WLF182 CHECKLIST REQUIREMENTS PRELIMINARY CONDOMINIUM APPRAISAL', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
                                                                                                                                                         

INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (21, N'HLA-V', N'HLF1035 Housing Loan Application (V01)', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (22, N'HLA-CB', N'HLF1036 Housing Loan Application Co-borrower', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (23, N'HLF1042', N'Borrower-Beneficiary Conformity', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (24, N'HLF1046', N'Authority to Deduct 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (25, N'HLF1069', N'Conformity Non-relatives 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (26, N'WLF252', N'Buyer Confirmation Form', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (27, N'PYSL', N'Payslip', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (28, N'COE', N'Certificate Of Employment', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (29, N'ITR', N'Latest Income Tax Return', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (30, N'COM-VCHER', N'Commision Voucher', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (31, N'BS', N'Bank Statement for the last 12 Months', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (32, N'CLC-TD', N'Copy of Lease Contact and Tax Declaration', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (33, N'CTC-TF', N'Certified True Copy of Transport Franchise', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (34, N'COENGMNT', N'Certificate of Engagement', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (35, N'', N'Statement of pressumed income to be certified by the barangay', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
SET IDENTITY_INSERT [dbo].[DocumentType] OFF
END
GO


IF NOT EXISTS (SELECT 1 FROM [dbo].DocumentVerification)
BEGIN
	SET IDENTITY_INSERT [dbo].DocumentVerification ON
INSERT INTO [dbo].[DocumentVerification] ([Id], DocumentTypeId, [Type], CreatedById, DateCreated, ModifiedById, DateModified)
VALUES 
    (1, 1, 1, 1, GETDATE(), NULL, NULL),
    (2, 2, 1, 1, GETDATE(), NULL, NULL), 
    (3, 3, 1, 1, GETDATE(), NULL, NULL), 
    (4, 4, 1, 1, GETDATE(), NULL, NULL), 
    (5, 5, 1, 1, GETDATE(), NULL, NULL), 
    (6, 6, 1, 1, GETDATE(), NULL, NULL), 
    (7, 7, 1, 1, GETDATE(), NULL, NULL),


    (8, 21, 2, 1, GETDATE(), NULL, NULL), 
    (9, 22, 2, 1, GETDATE(), NULL, NULL), 
    (10, 23, 2, 1, GETDATE(), NULL, NULL),
    (11, 24, 2, 1, GETDATE(), NULL, NULL), 
    (12, 25, 2, 1, GETDATE(), NULL, NULL), 
    (13, 26, 2, 1, GETDATE(), NULL, NULL),
    (14, 27, 1, 1, GETDATE(), NULL, NULL), 
    (15, 28, 1, 1, GETDATE(), NULL, NULL), 
    (16, 29, 1, 1, GETDATE(), NULL, NULL), 
    (17, 30, 1, 1, GETDATE(), NULL, NULL), 
    (18, 31, 1, 1, GETDATE(), NULL, NULL), 
    (19, 32, 1, 1, GETDATE(), NULL, NULL), 
    (20, 33, 1, 1, GETDATE(), NULL, NULL), 
    (21, 34, 1, 1, GETDATE(), NULL, NULL), 
    (22, 35, 1, 1, GETDATE(), NULL, NULL); 

 
SET IDENTITY_INSERT [dbo].DocumentVerification OFF

END
GO 




IF NOT EXISTS (SELECT 1 FROM [dbo].SubDocument)
BEGIN
	SET IDENTITY_INSERT [dbo].SubDocument ON
INSERT INTO [dbo].[SubDocument] ([Id], DocumentTypeId, [Type], ParentId, DateCreated, DateModified)
VALUES 
    (1, 27, 1, 2, GETDATE(), NULL), 
    (2, 28, 2, 2, GETDATE(), NULL), 
    (3, 29, 2, 2, GETDATE(), NULL), 
    (4, 30, 2, 2, GETDATE(), NULL), 
    (5, 31, 2, 2, GETDATE(), NULL), 
    (6, 32, 2, 2, GETDATE(), NULL), 
    (7, 33, 2, 2, GETDATE(), NULL), 
    (8, 34, 2, 2, GETDATE(), NULL), 
    (9, 35, 3, 2, GETDATE(), NULL); 

 
SET IDENTITY_INSERT [dbo].SubDocument OFF

END
GO 