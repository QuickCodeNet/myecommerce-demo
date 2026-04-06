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
    public class DeleteApiMethodDefinitionsWithModuleNameCommand : IRequest<Response<int>>
    {
        public string ApiMethodDefinitionModuleName { get; set; }

        public DeleteApiMethodDefinitionsWithModuleNameCommand(string apiMethodDefinitionModuleName)
        {
            this.ApiMethodDefinitionModuleName = apiMethodDefinitionModuleName;
        }

        public class DeleteApiMethodDefinitionsWithModuleNameHandler : IRequestHandler<DeleteApiMethodDefinitionsWithModuleNameCommand, Response<int>>
        {
            private readonly ILogger<DeleteApiMethodDefinitionsWithModuleNameHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public DeleteApiMethodDefinitionsWithModuleNameHandler(ILogger<DeleteApiMethodDefinitionsWithModuleNameHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(DeleteApiMethodDefinitionsWithModuleNameCommand request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.DeleteApiMethodDefinitionsWithModuleNameAsync(request.ApiMethodDefinitionModuleName);
                return returnValue.ToResponse();
            }
        }
    }
}