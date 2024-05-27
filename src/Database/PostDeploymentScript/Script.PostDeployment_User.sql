 IF NOT EXISTS (SELECT 1 FROM [dbo].[User])
BEGIN
    -- Enable IDENTITY_INSERT
    SET IDENTITY_INSERT [dbo].[User] ON;

    -- Insert statements
    INSERT INTO [dbo].[User] (
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
        [IsDark],
        [PagibigNumber]
    ) 
    VALUES 
        (1, N'Admin', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', N'Super', N'Admin', N'', N'admin@email.com', N'Admin', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, 0, NULL),
        (2, N'LGU', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', N'Oliver', N'Jay pee', N'L', N'lgu@email.com', N'LGU', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, 0, NULL),
        (3, N'Pagibig', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', N'Rosales', N'Mike', N'C', N'Pagibig@email.com', N'Pag-ibig', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, 0, NULL),
        (4, N'Beneficiary', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', N'Cortel', N'Albert', N'La Viña', N'beneficiary@email.com', N'Beneficiary', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, 0, N'324135645768'),
        (5, N'Developer', N'b5lj1xrAEY90IM7voc61Dg8HDyREnU2mwQTl8sozfA8=', N'your-password-salt', N'Concepcion', N'David', N'C', N'developer@email.com', N'Developer', 0, 1, CAST(N'2023-05-31T10:35:44.1310781' AS DateTime2), NULL, 0, NULL);

    -- Disable IDENTITY_INSERT
    SET IDENTITY_INSERT [dbo].[User] OFF;
END;
GO
