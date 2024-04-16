 
IF NOT EXISTS (SELECT 1 FROM [dbo].[ModuleType])
BEGIN
	SET IDENTITY_INSERT [dbo].[ModuleType] ON
	
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (1, N'Dashboard', N'<i class="mdi mdi-view-dashboard"></i>', 1, N'Home', N'Index', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (2, N'Create', N'<i class="fe-plus me-1"></i>', 2, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (3, N'Applicants', N'<i class="mdi mdi-folder-multiple"</i>', 3, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (4, N'Reports', N'<i class="mdi mdi-file-chart-outline me-1"></i>', 4, N'Report', N'Index', CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (5, N'Admin Panel', N'<i class="mdi mdi-cogs me-1"></i>', 5, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (6, N'Settings', N'<i class="mdi mdi-cogs me-1"></i>', 6, NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	
	SET IDENTITY_INSERT [dbo].[ModuleType] OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Module])
BEGIN
	SET IDENTITY_INSERT [dbo].[Module] ON

	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (1, N'HOME', N'Dashboard', 1, 0, N'<i class="mdi mdi-view-dashboard"></i>', N'Home', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, '2023-12-07 17:40:19.3000000')
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (2, N'MODULE', N'Modules', 5, 4, N'<i class="mdi mdi-format-list-text"></i>', N'Module', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (3, N'USER', N'User', 5, 2, N'', N'User', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (4, N'USER-ACC', N'User Management', 5, 1, N'<i class="mdi mdi-account-cog"></i>', N'User', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (5, N'ROLE', N'Role Management', 5, 2, N'<i class="mdi mdi-account-supervisor"></i>', N'Role', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (6, N'APPROVER-MGMT', N'Approval Scheme', 5, 3, N'<i class="mdi mdi-book-check"></i>', N'Approval', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (7, N'BNF-MGMT', N'Application Encoding', 3, 1,N'<i class="mdi mdi-folder-multiple-outline"></i>', N'Beneficiary', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (8, N'APLCNTREQ', N'Applications', 3, 0,N'<i class="mdi mdi-folder-account-outline"></i>', N'Applicants', N'ApplicantRequests', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (9, N'COMP', N'Company', 5, 6,  N'<i class="mdi mdi-domain"></i>', N'CompanyProfile',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (10, N'DOCMT', N'Requirements Setup', 5, 0, N'<i class="mdi mdi-content-save-settings"></i>', N'Document',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (11, N'FAQS', N'FAQS', 6, 2, N'<i class="mdi mdi-format-list-text"></i>', N'FAQS',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (12, N'BNF-PROFILE', N'My Application', 1, 1, N'<i class="mdi mdi-folder-account-outline"></i>', N'Applicants',  N'Beneficiary', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (13, N'APLHLF-NEW', N'Housing Loan Form', 1, 2, N'<i class="mdi mdi-form-select"></i>', N'Applicants',  N'HousingLoanForm', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)

	

	SET IDENTITY_INSERT [dbo].[Module] OFF
END
GO

  
IF NOT EXISTS (SELECT 1 FROM [dbo].[ModuleStage])
BEGIN
SET IDENTITY_INSERT [dbo].[ModuleStage] ON;
INSERT INTO [dbo].[ModuleStage] ([Id], [ModuleId], [Code], [Name], [Title], [Level], [ApproveDesc], [RejectDesc], [ReturnStage], [RequiredCount], [IsDisabled], [CreatedById], [DateCreated], [ModifiedById], [DateModified])
VALUES
    (1, 8, 'APLCNTREQ', 'Applicants Requests', 'Initial Stage', 1, 'Approved', 'Rejected', 0, 1 , 0, 1, '2024-03-19 09:00:00', NULL, NULL),
    (2, 8, 'APLCNTREQ', 'Applicants Requests', 'Final Stage', 2, 'Approved', 'Rejected', 0, 1, 0, 1, '2024-03-19 09:15:00', NULL, NULL)
 SET IDENTITY_INSERT [dbo].[ModuleStage] OFF
END
GO


 IF NOT EXISTS (SELECT 1 FROM [dbo].[ModuleStageApprover])
BEGIN
SET IDENTITY_INSERT [dbo].[ModuleStageApprover] ON;

INSERT INTO [dbo].[ModuleStageApprover] ([Id], [ModuleStageId], [ApproverId], [RoleId], [IsDisabled], [CreatedById], [DateCreated], [ModifiedById], [DateModified])
VALUES (1, 1, NULL, 5, 0, 1, GETDATE(), NULL, NULL);


INSERT INTO [dbo].[ModuleStageApprover] ([Id], [ModuleStageId], [ApproverId], [RoleId], [IsDisabled], [CreatedById], [DateCreated], [ModifiedById], [DateModified])
VALUES (2, 2, NULL, 3, 0, 1, GETDATE(), NULL, NULL);

SET IDENTITY_INSERT [dbo].[ModuleStageApprover] OFF;
END
GO