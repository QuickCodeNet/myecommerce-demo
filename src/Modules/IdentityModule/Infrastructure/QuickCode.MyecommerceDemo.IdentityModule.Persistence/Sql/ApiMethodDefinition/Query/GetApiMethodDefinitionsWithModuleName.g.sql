SELECT AMD.[Key], AMD.[ModuleName], AMD.[ModelName], AMD.[HttpMethod], AMD.[ControllerName], AMD.[MethodName], AMD.[UrlPath] 
FROM [ApiMethodDefinitions] AMD 
WHERE AMD.[ModuleName] = @PRM_ApiMethodDefinition_ModuleName 
ORDER BY AMD.[Key] 