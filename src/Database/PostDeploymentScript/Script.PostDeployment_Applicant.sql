 

 IF NOT EXISTS (SELECT 1 FROM [dbo].ApplicantsPersonalInformation)
BEGIN
SET IDENTITY_INSERT ApplicantsPersonalInformation ON;

-- Insert dummy data into ApplicantsPersonalInformation
INSERT INTO ApplicantsPersonalInformation
    (Id, Code, UserId, PagibigNumber, CompanyId, ApprovalStatus, DateCreated,CreatedById)
VALUES
    (1, 'APL202401-0001', 4, '324135645768', 1, 0, GETDATE(),1);

 
SET IDENTITY_INSERT ApplicantsPersonalInformation OFF;
END 
GO
 
 IF NOT EXISTS (SELECT 1 FROM [dbo].BarrowersInformation)
BEGIN
SET IDENTITY_INSERT BarrowersInformation ON;

-- Insert dummy data into BarrowersInformation
INSERT INTO BarrowersInformation
    (Id, ApplicantsPersonalInformationId, LastName, FirstName, MiddleName, Sex, PagibigMidNumber, Email,CreatedById)
VALUES
    (1, 1, 'Cortel', 'Albert', 'La Vina', 'Male', '324135645768', 'beneficiary@email.com',1);
    

-- Disable identity insert for BarrowersInformation
SET IDENTITY_INSERT BarrowersInformation OFF;
END 
GO



 IF NOT EXISTS (SELECT 1 FROM [dbo].ApprovalStatus)
BEGIN
SET IDENTITY_INSERT ApprovalStatus ON;
 
 
INSERT INTO ApprovalStatus
(Id,UserId,ReferenceId,ReferenceType,[Status],LastUpdate)
VALUES(1,4,1,8,1,GETDATE())
 
SET IDENTITY_INSERT ApprovalStatus OFF;
END 
GO


