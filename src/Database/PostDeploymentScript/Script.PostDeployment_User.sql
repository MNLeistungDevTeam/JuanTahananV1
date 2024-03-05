IF NOT EXISTS (SELECT 1 FROM [dbo].[User])
BEGIN
	SET IDENTITY_INSERT [dbo].[User] ON 

	INSERT [dbo].[User] (
		[Id],
		[UserName],
		[Password],
		[PasswordSalt],
		[LastName],
		[FirstName], 
		[MiddleName],
		[Email], 
		[Position],
		[IsDisabled],
		[CreatedById],
		[DateCreated], 
		[DateModified],
		[IsDark]) 
	VALUES (1, N'Admin', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', 'Super', 'Admin', '', N'admin@email.com', 'Admin', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL,0),
	       (2, N'Beneficiary', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', 'Cortel', 'Albert', 'La Viña', N'beneficiary@email.com', 'Beneficiary', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL,0),
		   (3, N'LGU', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', 'Oliver', 'Jay pee', 'L', N'lgu@email.com', 'LGU', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL,0),
		   (4, N'Pagibig', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', 'Rosales', 'Mike', 'C', N'Pagibig@email.com', 'Pag-ibig', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL,0)
	SET IDENTITY_INSERT [dbo].[User] OFF
END
GO