 IF NOT EXISTS (SELECT 1 FROM [dbo].ModeOfPayment)
 BEGIN

 SET IDENTITY_INSERT [dbo].ModeOfPayment ON 
 INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (1,'Salary Deduction',GETDATE(),1)
 

INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (2,'Over-the-Counter',GETDATE(),1)
 

INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (3,'Collecting Agent',GETDATE(),1)
 

INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (4,'Bank',GETDATE(),1)
 

INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (5,'Post-Dated Checks',GETDATE(),1)
INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (6,'Cash/Check',GETDATE(),1)
INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (7,'Developer',GETDATE(),1)
INSERT [dbo].ModeOfPayment ([Id], [Description], [DateCreated], [CreatedById]) VALUES (8,'Remittance Center',GETDATE(),1)
SET IDENTITY_INSERT [dbo].ModeOfPayment OFF 
END
GO