/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
IF NOT EXISTS (SELECT 1 FROM [dbo].DocumentVerification)
BEGIN
	SET IDENTITY_INSERT [dbo].DocumentVerification ON
 -- Insert dummy data with document type ids 5, 3, 4, 6, 7, 8, and 9
INSERT INTO [dbo].[DocumentVerification] ([Id], DocumentTypeId, [Type], CreatedById, DateCreated, ModifiedById, DateModified)
VALUES 
    (1, 5, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 1
    (2, 3, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 2
    (3, 4, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 3
    (4, 6, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 4
    (5, 7, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 5
    (6, 8, 1, 1, GETDATE(), NULL, NULL), -- Example dummy data 6
    (7, 9, 1, 1, GETDATE(), NULL, NULL); -- Example dummy data 7

SET IDENTITY_INSERT [dbo].DocumentVerification OFF

END
GO