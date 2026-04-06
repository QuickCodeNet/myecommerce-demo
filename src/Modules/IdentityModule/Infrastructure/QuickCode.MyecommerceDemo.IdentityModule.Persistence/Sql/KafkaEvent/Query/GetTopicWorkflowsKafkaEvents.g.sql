SELECT T.[Id], T.[KafkaEventsTopicName], T.[WorkflowContent] 
FROM [KafkaEvents] K 
	INNER JOIN [TopicWorkflows] T 
			ON T.[KafkaEventsTopicName] = K.[TopicName] 
WHERE T.[IsDeleted] = 0 
	AND K.[TopicName] = @PRM_KafkaEvents_TopicName 
ORDER BY T.[Id] 
OFFSET @StartIndex ROWS FETCH NEXT @PageSize ROWS ONLY 