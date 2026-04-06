UPDATE [RefreshTokens] 
	SET [IsRevoked] = 1 
WHERE [IsDeleted] = 0 
	AND [Token] = @PRM_RefreshToken_Token