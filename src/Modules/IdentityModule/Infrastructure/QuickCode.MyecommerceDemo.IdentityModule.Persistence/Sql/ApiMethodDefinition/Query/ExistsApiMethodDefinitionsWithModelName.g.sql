SELECT CASE WHEN EXISTS (
SELECT 1 
FROM [ApiMethodDefinitions] AMD 
WHERE AMD.[ModelName] = @PRM_ApiMethodDefinition_ModelName ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END