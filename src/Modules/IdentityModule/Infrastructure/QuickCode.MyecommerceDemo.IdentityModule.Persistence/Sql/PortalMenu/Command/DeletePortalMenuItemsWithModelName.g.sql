DELETE FROM [PortalMenus] 
WHERE [Key] LIKE @PRM_PortalMenu_Key + '%' 
	AND [Name] = @PRM_PortalMenu_Name