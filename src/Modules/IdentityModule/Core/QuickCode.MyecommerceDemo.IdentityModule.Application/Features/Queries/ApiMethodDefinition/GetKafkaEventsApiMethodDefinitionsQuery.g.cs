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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodDefinition;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodDefinition
{
    public class GetKafkaEventsApiMethodDefinitionsQuery : IRequest<Response<List<GetKafkaEventsApiMethodDefinitionsResponseDto>>>
    {
        public string ApiMethodDefinitionsKey { get; set; }
        public int? pageNumber { get; set; }
        public int? pageSize { get; set; }

        public GetKafkaEventsApiMethodDefinitionsQuery(string apiMethodDefinitionsKey, int? pageNumber, int? pageSize)
        {
            this.ApiMethodDefinitionsKey = apiMethodDefinitionsKey;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

        public class GetKafkaEventsApiMethodDefinitionsHandler : IRequestHandler<GetKafkaEventsApiMethodDefinitionsQuery, Response<List<GetKafkaEventsApiMethodDefinitionsResponseDto>>>
        {
            private readonly ILogger<GetKafkaEventsApiMethodDefinitionsHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public GetKafkaEventsApiMethodDefinitionsHandler(ILogger<GetKafkaEventsApiMethodDefinitionsHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetKafkaEventsApiMethodDefinitionsResponseDto>>> Handle(GetKafkaEventsApiMethodDefinitionsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetKafkaEventsApiMethodDefinitionsAsync(request.ApiMethodDefinitionsKey, request.pageNumber, request.pageSize);
                return returnValue.ToResponse();
            }
        }
    }
}