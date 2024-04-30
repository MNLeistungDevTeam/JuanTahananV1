
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
:r .\PostDeploymentScript\Script.PostDeployment_User.sql
:r .\PostDeploymentScript\Script.PostDeployment_ModuleStatus.sql
:r .\PostDeploymentScript\Script.PostDeployment_Module.sql
:r .\PostDeploymentScript\Script.PostDeployment_Role.sql
:r .\PostDeploymentScript\Script.PostDeployment_RoleAccess.sql
:r .\PostDeploymentScript\Script.PostDeployment_UserRole.sql
:r .\PostDeploymentScript\Script.PostDeployment_DocumentType.sql
:r .\PostDeploymentScript\Script.PostDeployment_PropertyType.sql
:r .\PostDeploymentScript\Script.PostDeployment_PurposeOfLoan.sql
:r .\PostDeploymentScript\Script.PostDeployment_FormatBytes.sql
:r .\PostDeploymentScript\Script.PostDeployment_Country.sql
:r .\PostDeploymentScript\Script.PostDeployment_AddressType.sql
:r .\PostDeploymentScript\Script.PostDeployment_ModeOfPayment.sql
:r .\PostDeploymentScript\Script.PostDeployment_Company.sql
:r .\PostDeploymentScript\Script.PostDeployment_SourcePagibigFund.sql
:r .\PostDeploymentScript\Script.PostDeployment_Industry.sql
:r .\PostDeploymentScript\Script.PostDeployment_Applicant.sql
:r .\PostDeploymentScript\Script.PostDeployment_EmailSetup.sql
