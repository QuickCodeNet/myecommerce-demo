SELECT PPD.[Key], PPD.[ModuleName], PPD.[ModelName], PPD.[PageAction], PPD.[PagePath] 
FROM [PortalPageDefinitions] PPD 
WHERE PPD.[ModelName] = @PRM_PortalPageDefinition_ModelName 
ORDER BY PPD.[Key] 