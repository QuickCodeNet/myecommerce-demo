SELECT CASE WHEN EXISTS (
SELECT 1 
FROM [ApiMethodDefinitions] AMD 
WHERE AMD.[ModuleName] = @PRM_ApiMethodDefinition_ModuleName ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END