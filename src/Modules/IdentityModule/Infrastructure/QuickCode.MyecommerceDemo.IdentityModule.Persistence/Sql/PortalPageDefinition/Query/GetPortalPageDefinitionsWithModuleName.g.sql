SELECT PPD.[Key], PPD.[ModuleName], PPD.[ModelName], PPD.[PageAction], PPD.[PagePath] 
FROM [PortalPageDefinitions] PPD 
WHERE PPD.[ModuleName] = @PRM_PortalPageDefinition_ModuleName 
ORDER BY PPD.[Key] 