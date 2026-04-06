DELETE K 
FROM [ApiMethodDefinitions] A 
	INNER JOIN [KafkaEvents] K 
			ON K.[ApiMethodDefinitionKey] = A.[Key] 
WHERE A.[ModuleName] = @PRM_ApiMethodDefinitions_ModuleName