SELECT AMAG.[PermissionGroupName], AMAG.[ApiMethodDefinitionKey], AMAG.[ModifiedBy], AMAG.[IsActive] 
FROM [ApiMethodAccessGrants] AMAG 
WHERE AMAG.[PermissionGroupName] = @PRM_ApiMethodAccessGrant_PermissionGroupName 
	AND AMAG.[IsActive] = 1 
ORDER BY AMAG.[PermissionGroupName], AMAG.[ApiMethodDefinitionKey] 