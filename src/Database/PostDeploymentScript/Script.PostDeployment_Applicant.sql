 

-- IF NOT EXISTS (SELECT 1 FROM [dbo].ApplicantsPersonalInformation)
--BEGIN
--SET IDENTITY_INSERT ApplicantsPersonalInformation ON;

---- Insert dummy data into ApplicantsPersonalInformation
--INSERT INTO ApplicantsPersonalInformation
--    (Id, Code, UserId, PagibigNumber, CompanyId, ApprovalStatus, DateCreated,CreatedById)
--VALUES
--    (1, 'APL202401-0001', 4, '324135645768', 1, 0, GETDATE(),1);

 
--SET IDENTITY_INSERT ApplicantsPersonalInformation OFF;
--END 
--GO
 
-- IF NOT EXISTS (SELECT 1 FROM [dbo].BarrowersInformation)
--BEGIN
--SET IDENTITY_INSERT BarrowersInformation ON;

---- Insert dummy data into BarrowersInformation
--INSERT INTO BarrowersInformation
--    (Id, ApplicantsPersonalInformationId, LastName, FirstName, MiddleName, Sex, PagibigMidNumber, Email,CreatedById)
--VALUES
--    (1, 1, 'Cortel', 'Albert', 'La Vina', 'Male', '324135645768', 'beneficiary@email.com',1);
    

---- Disable identity insert for BarrowersInformation
--SET IDENTITY_INSERT BarrowersInformation OFF;
--END 
--GO




 IF NOT EXISTS (SELECT 1 FROM [dbo].BeneficiaryInformation)
BEGIN
SET IDENTITY_INSERT BeneficiaryInformation ON;

-- Insert dummy data into BarrowersInformation
--INSERT INTO BeneficiaryInformation
--    (Id,Code, UserId,PagibigNumber, LastName, FirstName, MiddleName, Sex, Email,CreatedById)
--VALUES
--    (1,'BNF202405-0001', 4,'324135645768', 'Cortel', 'Albert', 'La Vina', 'Male', 'beneficiary@email.com',1);

 INSERT INTO BeneficiaryInformation
 (Id,UserId,Code,PagibigNumber, LastName, FirstName, MiddleName, Sex, Age, BirthDate, Email, MobileNumber,
  PresentUnitName, PresentBuildingName, PresentLotName, PresentStreetName, PresentSubdivisionName, PresentBaranggayName,
  PresentMunicipalityName, PresentProvinceName, PresentZipCode, PermanentUnitName, PermanentBuildingName, PermanentLotName,
  PermanentStreetName, PermanentSubdivisionName, PermanentBaranggayName, PermanentMunicipalityName, PermanentProvinceName,
  PermanentZipCode, PropertyDeveloperName, PropertyUnitLevelName, PropertyLocation,CreatedById)
  VALUES 
  (1,4,'BNF202405-0001','324135645768', 'Cortel', 'Albert', 'La Vina', 'Male', '23','1990-01-01', 'beneficiary@email.com', '09458643650',
    '123', 'Example Building', '456', 'Main Street', 'Sample Subdivision', 'Baranggay Name', 'Sample City', 'Sample Province', '12345',
    '789', 'Permanent Building', '101', 'Permanent Street', 'Permanent Subdivision', 'Permanent Baranggay', 'Permanent City', 
    'Permanent Province', '54321', 'RS Realty Developer Inc.', '2 BEDROOM 24 SQM', 'Bacolod',1);



    

-- Disable identity insert for BarrowersInformation
SET IDENTITY_INSERT BeneficiaryInformation OFF;
END 
GO


-- IF NOT EXISTS (SELECT 1 FROM [dbo].ApprovalStatus)
--BEGIN
--SET IDENTITY_INSERT ApprovalStatus ON;
 
 
--INSERT INTO ApprovalStatus
--(Id,UserId,ReferenceId,ReferenceType,[Status],LastUpdate)
--VALUES(1,4,1,8,1,GETDATE())
 
--SET IDENTITY_INSERT ApprovalStatus OFF;
--END 
--GO


