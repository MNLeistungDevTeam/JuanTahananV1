CREATE PROCEDURE [dbo].[spModuleStage_GetByModuleId]
	@id INT
AS
BEGIN
	 SELECT
    ms.*,
    m.[Description] AS ModuleName,
    m.[Code] AS ModuleCode,
    msa.RoleId,
    msa.ApproverId,
    CASE 
        WHEN msa.RoleId = 0 OR msa.RoleId IS NULL THEN CONCAT(u.FirstName, ' ', ISNULL(u.MiddleName, ''), ' ', u.LastName)
        ELSE r.[Name]
    END AS ApproverName
      
FROM 
    ModuleStage ms
INNER JOIN 
    Module m ON m.Id = ms.ModuleId
INNER JOIN 
    ModuleStageApprover msa ON msa.ModuleStageId = ms.Id
LEFT JOIN 
    [Role] r ON r.Id = msa.RoleId
LEFT JOIN 
    [User] u ON u.Id = msa.ApproverId -- assuming 'u' is an alias for the User table
WHERE 
    m.Id = @id
ORDER BY 
    ms.[Level];
END
