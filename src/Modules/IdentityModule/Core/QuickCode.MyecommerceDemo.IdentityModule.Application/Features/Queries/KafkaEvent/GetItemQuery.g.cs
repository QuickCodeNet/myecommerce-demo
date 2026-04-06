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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.KafkaEvent;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.KafkaEvent
{
    public class GetItemKafkaEventQuery : IRequest<Response<KafkaEventDto>>
    {
        public string TopicName { get; set; }

        public GetItemKafkaEventQuery(string topicName)
        {
            this.TopicName = topicName;
        }

        public class GetItemKafkaEventHandler : IRequestHandler<GetItemKafkaEventQuery, Response<KafkaEventDto>>
        {
            private readonly ILogger<GetItemKafkaEventHandler> _logger;
            private readonly IKafkaEventRepository _repository;
            public GetItemKafkaEventHandler(ILogger<GetItemKafkaEventHandler> logger, IKafkaEventRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<KafkaEventDto>> Handle(GetItemKafkaEventQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.TopicName);
                return returnValue.ToResponse();
            }
        }
    }
}