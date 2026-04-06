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
    public class ListKafkaEventQuery : IRequest<Response<List<KafkaEventDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListKafkaEventQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListKafkaEventHandler : IRequestHandler<ListKafkaEventQuery, Response<List<KafkaEventDto>>>
        {
            private readonly ILogger<ListKafkaEventHandler> _logger;
            private readonly IKafkaEventRepository _repository;
            public ListKafkaEventHandler(ILogger<ListKafkaEventHandler> logger, IKafkaEventRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<KafkaEventDto>>> Handle(ListKafkaEventQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}