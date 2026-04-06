using System;
using System.Linq;
using QuickCode.MyecommerceDemo.Common.Mediator;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.TopicWorkflow;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.TopicWorkflow
{
    public class GetWorkflows2Query : IRequest<Response<List<GetWorkflows2ResponseDto>>>
    {
        public string TopicWorkflowKafkaEventsTopicName { get; set; }

        public GetWorkflows2Query(string topicWorkflowKafkaEventsTopicName)
        {
            this.TopicWorkflowKafkaEventsTopicName = topicWorkflowKafkaEventsTopicName;
        }

        public class GetWorkflows2Handler : IRequestHandler<GetWorkflows2Query, Response<List<GetWorkflows2ResponseDto>>>
        {
            private readonly ILogger<GetWorkflows2Handler> _logger;
            private readonly ITopicWorkflowRepository _repository;
            public GetWorkflows2Handler(ILogger<GetWorkflows2Handler> logger, ITopicWorkflowRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetWorkflows2ResponseDto>>> Handle(GetWorkflows2Query request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetWorkflows2Async(request.TopicWorkflowKafkaEventsTopicName);
                return returnValue.ToResponse();
            }
        }
    }
}