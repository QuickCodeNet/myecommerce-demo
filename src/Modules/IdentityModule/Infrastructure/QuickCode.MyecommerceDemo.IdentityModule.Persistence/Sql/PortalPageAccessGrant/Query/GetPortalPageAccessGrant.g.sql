SELECT PPAG.[PermissionGroupName], PPAG.[PortalPageDefinitionKey], PPAG.[PageAction], PPAG.[ModifiedBy], PPAG.[IsActive] 
FROM [PortalPageAccessGrants] PPAG 
WHERE PPAG.[PortalPageDefinitionKey] = @PRM_PortalPageAccessGrant_PortalPageDefinitionKey 
	AND PPAG.[PermissionGroupName] = @PRM_PortalPageAccessGrant_PermissionGroupName 
	AND PPAG.[PageAction] = @PRM_PortalPageAccessGrant_PageAction 
	AND PPAG.[IsActive] = 1 
ORDER BY PPAG.[PermissionGroupName], PPAG.[PortalPageDefinitionKey], PPAG.[PageAction] 