SELECT K.[TopicName], K.[ApiMethodDefinitionKey], K.[IsActive], A.[HttpMethod], A.[ControllerName], A.[UrlPath] 
FROM [KafkaEvents] K 
	INNER JOIN [ApiMethodDefinitions] A 
			ON K.[ApiMethodDefinitionKey] = A.[Key] 
WHERE K.[IsActive] = 1 
ORDER BY K.[TopicName] 