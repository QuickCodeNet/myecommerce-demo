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
    public class GetActiveKafkaEventsQuery : IRequest<Response<List<GetActiveKafkaEventsResponseDto>>>
    {
        public bool KafkaEventsIsActive { get; set; }

        public GetActiveKafkaEventsQuery(bool kafkaEventsIsActive)
        {
            this.KafkaEventsIsActive = kafkaEventsIsActive;
        }

        public class GetActiveKafkaEventsHandler : IRequestHandler<GetActiveKafkaEventsQuery, Response<List<GetActiveKafkaEventsResponseDto>>>
        {
            private readonly ILogger<GetActiveKafkaEventsHandler> _logger;
            private readonly IKafkaEventRepository _repository;
            public GetActiveKafkaEventsHandler(ILogger<GetActiveKafkaEventsHandler> logger, IKafkaEventRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetActiveKafkaEventsResponseDto>>> Handle(GetActiveKafkaEventsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetActiveKafkaEventsAsync(request.KafkaEventsIsActive);
                return returnValue.ToResponse();
            }
        }
    }
}