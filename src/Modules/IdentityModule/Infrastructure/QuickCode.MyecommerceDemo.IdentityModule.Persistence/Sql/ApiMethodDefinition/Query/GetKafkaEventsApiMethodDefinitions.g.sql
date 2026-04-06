SELECT K.[TopicName], K.[ApiMethodDefinitionKey], K.[IsActive] 
FROM [ApiMethodDefinitions] A 
	INNER JOIN [KafkaEvents] K 
			ON K.[ApiMethodDefinitionKey] = A.[Key] 
WHERE A.[Key] = @PRM_ApiMethodDefinitions_Key 
ORDER BY K.[TopicName] 
OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY 