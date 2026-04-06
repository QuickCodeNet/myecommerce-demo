SELECT AMD.[Key], AMD.[ModuleName], AMD.[ModelName], AMD.[HttpMethod], AMD.[ControllerName], AMD.[MethodName], AMD.[UrlPath] 
FROM [ApiMethodDefinitions] AMD 
WHERE AMD.[ModelName] = @PRM_ApiMethodDefinition_ModelName 
ORDER BY AMD.[Key] 