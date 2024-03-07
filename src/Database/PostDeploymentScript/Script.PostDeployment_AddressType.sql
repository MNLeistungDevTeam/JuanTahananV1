IF NOT EXISTS (SELECT 1 FROM [dbo].[AddressType])
BEGIN
 INSERT INTO [dbo].[AddressType]
           ([Code]
           ,[Name]
           ,CreatedById
           ,[DateCreated])
     VALUES
      ('AT001', 'Home Address', 1, CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2)),
      ('AT002', 'Office Address', 1, CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2)),
      ('AT003', 'Business Address',  1, CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2)),
      ('AT004', 'Billing Address', 1, CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2)),
      ('AT005', 'Shipping Address', 1, CAST(N'2023-06-01T00:00:00.0000000' AS DateTime2))
END
GO