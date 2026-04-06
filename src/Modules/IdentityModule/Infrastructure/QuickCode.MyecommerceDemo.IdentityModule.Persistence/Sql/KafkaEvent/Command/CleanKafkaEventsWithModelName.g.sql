DELETE K 
FROM [ApiMethodDefinitions] A 
	INNER JOIN [KafkaEvents] K 
			ON K.[ApiMethodDefinitionKey] = A.[Key] 
WHERE A.[ModelName] = @PRM_ApiMethodDefinitions_ModelName