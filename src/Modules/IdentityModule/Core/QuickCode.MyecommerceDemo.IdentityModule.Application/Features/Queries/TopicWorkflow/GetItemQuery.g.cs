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
    public class GetItemTopicWorkflowQuery : IRequest<Response<TopicWorkflowDto>>
    {
        public int Id { get; set; }

        public GetItemTopicWorkflowQuery(int id)
        {
            this.Id = id;
        }

        public class GetItemTopicWorkflowHandler : IRequestHandler<GetItemTopicWorkflowQuery, Response<TopicWorkflowDto>>
        {
            private readonly ILogger<GetItemTopicWorkflowHandler> _logger;
            private readonly ITopicWorkflowRepository _repository;
            public GetItemTopicWorkflowHandler(ILogger<GetItemTopicWorkflowHandler> logger, ITopicWorkflowRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<TopicWorkflowDto>> Handle(GetItemTopicWorkflowQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Id);
                return returnValue.ToResponse();
            }
        }
    }
}