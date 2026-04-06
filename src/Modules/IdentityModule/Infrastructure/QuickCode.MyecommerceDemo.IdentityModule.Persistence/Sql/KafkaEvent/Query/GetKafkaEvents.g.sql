SELECT K.[TopicName], K.[ApiMethodDefinitionKey], K.[IsActive], A.[HttpMethod], A.[ControllerName], A.[UrlPath] 
FROM [KafkaEvents] K 
	INNER JOIN [ApiMethodDefinitions] A 
			ON K.[ApiMethodDefinitionKey] = A.[Key] 
ORDER BY K.[TopicName] 