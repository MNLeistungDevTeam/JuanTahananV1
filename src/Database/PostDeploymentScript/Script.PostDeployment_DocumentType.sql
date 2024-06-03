IF NOT EXISTS (SELECT 1 FROM [dbo].[DocumentType])
BEGIN
SET IDENTITY_INSERT [dbo].[DocumentType] ON 
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (1, N'PAGIBIG-HLF', N'Pag-IBIG Housing Loan Application (HLF)', CAST(N'2024-02-24T08:31:43.3808888' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (2,  N'PROOF-INCM',N'Proof Of Income', CAST(N'2024-02-24T08:31:57.0094831' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES (3, N'PAGIBIG-HLAW/RCNTVALID-ID', N'Recent Valid ID (borrower & co-borrower) together w/ the HLF', CAST(N'2024-02-24T08:32:05.9640793' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (4, N'BC', N'Birth Certificate', CAST(N'2024-02-24T08:32:12.9360798' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (5, N'MC', N'Marriage Certificate Or Cenomar', CAST(N'2024-02-24T08:32:21.6886051' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (6, N'CD', N'Cedula', CAST(N'2024-02-24T08:32:32.3003803' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (7, N'ID', N'Recent Valid ID of Borrower & co-borrower', CAST(N'2024-02-24T08:32:44.9771157' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (8, N'TCT', N'TCT (Transfer Certificate of Title)', CAST(N'2024-02-24T08:32:59.0298253' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (9, N'CCT', N'CCT (Condominium Certificate of Title)', CAST(N'2024-02-24T08:33:06.3514489' AS DateTime2), 1, CAST(N'2024-02-24T08:33:46.5876187' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (10, N'HLA-CB', N'HLF069 Housing Loan Application Co-Borrower', CAST(N'2024-02-24T08:33:20.1570733' AS DateTime2), 1, CAST(N'2024-02-24T08:33:56.1223448' AS DateTime2), 1, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (11, N'BVS-DA', N'HLF058 Borrowers Validation Sheet Developer Assisted', CAST(N'2024-02-24T08:34:16.8148136' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (12, N'DSC', N'HLF062 Developer Sworn Certification', CAST(N'2024-02-24T08:34:26.8207202' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (13, N'SPA', N'HLF064 Special Power of Attorney', CAST(N'2024-02-24T08:34:38.5568817' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (14, N'CA', N'HLF083 Certificate of Acceptance', CAST(N'2024-02-24T08:34:46.8078092' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (15, N'PN-HF', N'HLF086 Promissory Note Home Financing Program', CAST(N'2024-02-24T08:34:53.7658978' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (16, N'PN-AH', N'HLF087 Promissory Note Affordable Housing Program', CAST(N'2024-02-24T08:35:00.7742975' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (17, N'DCS-EP', N'HLF234 DCS Enduser Program', CAST(N'2024-02-24T08:35:20.8384282' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (18, N'DCS-AH', N'HLF235 DCS Affordable Housing', CAST(N'2024-02-24T08:35:28.3170334' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (19, N'DOAS-RH', N'HLF236 DOAS Regular Housing', CAST(N'2024-02-24T08:35:38.3197435' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (20, N'CR-PCA', N'WLF182 Checklist Requirements Preliminary Condominium Appraisal', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
                                                                                                                                                         




INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (21, N'HLA-V', N'HLF1035 Housing Loan Application (V01)', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (22, N'HLA-CB', N'HLF1036 Housing Loan Application Co-borrower', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (23, N'HLF1042', N'Borrower-Beneficiary Conformity', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (24, N'HLF1046', N'Authority to Deduct 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (25, N'HLF1069', N'Conformity Non-relatives 4PH', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL)
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById]) VALUES  (26, N'WLF252', N'4PH Buyer Confirmation Form', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (27, N'PYSL', N'Payslip', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (28, N'COE', N'Certificate Of Employment', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (29, N'ITR', N'Latest Income Tax Return', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (30, N'COM-VCHER', N'Commision Voucher', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (31, N'BS', N'Bank Statement for the last 12 months', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (32, N'CLC-TD', N'Copy of Lease Contact and Tax Declaration', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (33, N'CTC-TF', N'Certified True Copy of Transport Franchise', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (34, N'COENGMNT', N'Certificate of Engagement', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (35, N'ST-BRGY', N'Statement of pressumed income to be certified by the barangay', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (36, N'HLF-1X1PIC', N'Recent Valid ID of Borrower & co-borrower (HLF)', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,2);

INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (37, N'4PH-HLF', N'4PH Housing Loan Application Form', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,NULL);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (38, N'4PH-HLF-CBRROWER', N'4PH Housing Loan Application Co-borrower', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,NULL);
INSERT [dbo].[DocumentType] ([Id],Code, [Description], [DateCreated], [CreatedById], [DateModified], [ModifiedById], [DateDeleted], [DeletedById],[ParentId]) VALUES  (39, N'BCF', N'Buyer Confirmation Form', CAST(N'2024-02-24T08:35:48.1374701' AS DateTime2), 1, NULL, NULL, NULL, NULL,NULL);

SET IDENTITY_INSERT [dbo].[DocumentType] OFF
END
GO


IF NOT EXISTS (SELECT 1 FROM [dbo].DocumentVerification)
BEGIN
	SET IDENTITY_INSERT [dbo].DocumentVerification ON
INSERT INTO [dbo].[DocumentVerification] ([Id], DocumentTypeId, [Type], CreatedById, DateCreated, ModifiedById, DateModified)
VALUES 
    (1, 1, 1, 1, GETDATE(), NULL, NULL),-- pagibig hloan
    (2, 3, 1, 1, GETDATE(), NULL, NULL), --recent valid id w coborrower
    (3, 26, 1, 1, GETDATE(), NULL, NULL), -- pagibig bcf
    (4, 4, 1, 1, GETDATE(), NULL, NULL), --b certificate
    (5, 5, 1, 1, GETDATE(), NULL, NULL),  --cenomar
    (6, 6, 1, 1, GETDATE(), NULL, NULL),  --cedula
    (7, 36, 1, 1, GETDATE(), NULL, NULL), -- . 1 x 1 ID Picture (Attached to HLF)

    (8, 27, 1, 1, GETDATE(), NULL, NULL),  -- PAYSLIP
    (9, 28, 1, 1, GETDATE(), NULL, NULL),  -- Coe
    (10, 29, 1, 1, GETDATE(), NULL, NULL), -- itr

    (11, 30, 1, 1, GETDATE(), NULL, NULL),  -- commision vouccher
    (12, 31, 1, 1, GETDATE(), NULL, NULL),  -- bank 12 months
    (13, 32, 1, 1, GETDATE(), NULL, NULL), -- copy lease tax decla
    (14, 33, 1, 1, GETDATE(), NULL, NULL), -- transport frnachise
    (15, 34, 1, 1, GETDATE(), NULL, NULL), -- coe
    (16, 35, 1, 1, GETDATE(), NULL, NULL), -- presume income


    (17, 37, 2, 1, GETDATE(), NULL, NULL),  -- 4PH  Housing Loan Application Form
    (18, 38, 2, 1, GETDATE(), NULL, NULL),  -- 4PH Housing Loan Application Co-borrower 
    (19, 23, 2, 1, GETDATE(), NULL, NULL), -- Borrower-Beneficiary Conformity
    (20, 24, 2, 1, GETDATE(), NULL, NULL), --  Authority to Deduct
    (21, 25, 2, 1, GETDATE(), NULL, NULL), -- Conformity Non-relatives
    (22, 39, 2, 1, GETDATE(), NULL, NULL); -- Buyer Confirmation Form


SET IDENTITY_INSERT [dbo].DocumentVerification OFF

END
GO 




IF NOT EXISTS (SELECT 1 FROM [dbo].SubDocument)
BEGIN
	SET IDENTITY_INSERT [dbo].SubDocument ON
INSERT INTO [dbo].[SubDocument] ([Id], DocumentTypeId, [Type], ParentId, DateCreated, DateModified)
VALUES 
    (1, 27, 1, 2, GETDATE(), NULL), 
    (2, 28, 1, 2, GETDATE(), NULL), 
    (3, 29, 1, 2, GETDATE(), NULL), 


  

    (4, 29, 2, 2, GETDATE(), NULL), 
    (5, 30, 2, 2, GETDATE(), NULL), 
    (6, 31, 2, 2, GETDATE(), NULL), 
    (7, 32, 2, 2, GETDATE(), NULL), 

    (8, 33, 2, 2, GETDATE(), NULL), 
    (9, 34, 2, 2, GETDATE(), NULL), 

    (10, 35, 3, 2, GETDATE(), NULL); 

 
SET IDENTITY_INSERT [dbo].SubDocument OFF

END
GO 