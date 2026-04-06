SELECT PPAG.[PermissionGroupName], PPAG.[PortalPageDefinitionKey], PPAG.[PageAction], PPAG.[ModifiedBy], PPAG.[IsActive] 
FROM [PortalPageAccessGrants] PPAG 
WHERE PPAG.[PermissionGroupName] = @PRM_PortalPageAccessGrant_PermissionGroupName 
	AND PPAG.[IsActive] = 1 
ORDER BY PPAG.[PermissionGroupName], PPAG.[PortalPageDefinitionKey], PPAG.[PageAction] 