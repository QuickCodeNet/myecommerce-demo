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
    public class ListTopicWorkflowQuery : IRequest<Response<List<TopicWorkflowDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListTopicWorkflowQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListTopicWorkflowHandler : IRequestHandler<ListTopicWorkflowQuery, Response<List<TopicWorkflowDto>>>
        {
            private readonly ILogger<ListTopicWorkflowHandler> _logger;
            private readonly ITopicWorkflowRepository _repository;
            public ListTopicWorkflowHandler(ILogger<ListTopicWorkflowHandler> logger, ITopicWorkflowRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<TopicWorkflowDto>>> Handle(ListTopicWorkflowQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}