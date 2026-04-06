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
    public class TotalCountApiMethodDefinitionQuery : IRequest<Response<int>>
    {
        public TotalCountApiMethodDefinitionQuery()
        {
        }

        public class TotalCountApiMethodDefinitionHandler : IRequestHandler<TotalCountApiMethodDefinitionQuery, Response<int>>
        {
            private readonly ILogger<TotalCountApiMethodDefinitionHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public TotalCountApiMethodDefinitionHandler(ILogger<TotalCountApiMethodDefinitionHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountApiMethodDefinitionQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}