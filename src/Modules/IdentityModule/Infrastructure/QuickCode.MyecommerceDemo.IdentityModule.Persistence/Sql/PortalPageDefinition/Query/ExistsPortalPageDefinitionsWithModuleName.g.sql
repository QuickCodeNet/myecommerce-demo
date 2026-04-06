SELECT CASE WHEN EXISTS (
SELECT 1 
FROM [PortalPageDefinitions] PPD 
WHERE PPD.[ModuleName] = @PRM_PortalPageDefinition_ModuleName ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END