 
IF NOT EXISTS (SELECT 1 FROM [dbo].[ModuleType])
BEGIN
	SET IDENTITY_INSERT [dbo].[ModuleType] ON
	
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (1, N'Dashboard', N'<i class="fe-airplay me-1"></i>', 1, N'Home', N'Index', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (2, N'Create', N'<i class="fe-plus me-1"></i>', 2, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (3, N'Applicants', N'<i class="mdi mdi-folder-multiple"</i>', 3, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (4, N'Reports', N'<i class="mdi mdi-file-chart-outline me-1"></i>', 4, N'Report', N'Index', CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (5, N'File Setup', N'<i class="mdi mdi-cogs me-1"></i>', 5, N'', N'', CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[ModuleType](Id, [Description], Icon, Ordinal, Controller, [Action], IsDisabled, IsVisible, InMaintenance, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (6, N'Settings', N'<i class="mdi mdi-cogs me-1"></i>', 6, NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	
	SET IDENTITY_INSERT [dbo].[ModuleType] OFF
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Module])
BEGIN
	SET IDENTITY_INSERT [dbo].[Module] ON

	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (1, N'HOME', N'Dashboard', 1, 0, N'<i class="fe-airplay me-1"></i>', N'Home', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, '2023-12-07 17:40:19.3000000')
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (2, N'MODULE', N'Module Management', 6, 0, NULL, N'Module', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (3, N'USER', N'User', 5, 2, N'', N'User', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (4, N'USER-ACC', N'User Account', 5, 0, N'', N'User', N'Index', 3, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (5, N'ROLE', N'Role Management', 5, 1, N'', N'Role', N'Index', 3, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 1, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (6, N'APPROVER-MGMT', N'Approver Management', 5, 2, NULL, N'ApproverManagement', N'Index', 3, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (7, N'BNF-MGMT', N'Benefiary Management', 3, 0,N'<i class="mdi mdi-format-list-text"></i>', N'Applicants', N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (8, N'APLCNTREQ', N'Applicants Requests', 3, 1,NULL, N'Applicants', N'ApplicantRequests', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'True'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (9, N'COMP', N'Company', 6, 1,  N'<i class="fe-airplay me-1"></i>', N'CompanyProfile',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (10, N'DOCMT', N'Document', 3, 2, NULL, N'Document',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)
	INSERT [dbo].[Module](Id, Code, [Description], ModuleTypeId, Ordinal, Icon, Controller, [Action], ParentModuleId, ApprovalRouteTypeId, IsDisabled, InMaintenance, IsVisible, WithApprover, CompanyId, CreatedById, DateCreated, ModifiedById, DateModified) VALUES (11, N'FAQS', N'FAQS', 6, 2, NULL, N'FAQS',  N'Index', NULL, NULL, CONVERT(bit, 'False'), CONVERT(bit, 'False'), CONVERT(bit, 'True'), CONVERT(bit, 'False'), 0, 1, GETDATE(), NULL, NULL)

	

	SET IDENTITY_INSERT [dbo].[Module] OFF
END
GO


