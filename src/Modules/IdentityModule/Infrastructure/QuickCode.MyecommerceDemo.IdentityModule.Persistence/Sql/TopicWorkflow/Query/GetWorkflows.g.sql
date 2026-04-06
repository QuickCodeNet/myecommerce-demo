SELECT TW.[Id], TW.[KafkaEventsTopicName], TW.[WorkflowContent] 
FROM [TopicWorkflows] TW 
WHERE TW.[IsDeleted] = 0 
	AND TW.[KafkaEventsTopicName] = @PRM_TopicWorkflow_KafkaEventsTopicName 
ORDER BY TW.[Id] 