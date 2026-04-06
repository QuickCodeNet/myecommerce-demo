SELECT TOP 1 RT.[Id], RT.[UserId], RT.[Token], RT.[ExpiryDate], RT.[CreatedDate], RT.[IsRevoked] 
FROM [RefreshTokens] RT 
WHERE RT.[IsDeleted] = 0 
	AND RT.[Token] = @PRM_RefreshToken_Token 
	AND RT.[IsRevoked] = 0 
	AND RT.[ExpiryDate] > GETDATE() 
ORDER BY RT.[Id] 