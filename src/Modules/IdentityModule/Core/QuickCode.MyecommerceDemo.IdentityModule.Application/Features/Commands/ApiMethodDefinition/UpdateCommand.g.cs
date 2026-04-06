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
    public class UpdateApiMethodDefinitionCommand : IRequest<Response<bool>>
    {
        public string Key { get; set; }
        public ApiMethodDefinitionDto request { get; set; }

        public UpdateApiMethodDefinitionCommand(string key, ApiMethodDefinitionDto request)
        {
            this.request = request;
            this.Key = key;
        }

        public class UpdateApiMethodDefinitionHandler : IRequestHandler<UpdateApiMethodDefinitionCommand, Response<bool>>
        {
            private readonly ILogger<UpdateApiMethodDefinitionHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public UpdateApiMethodDefinitionHandler(ILogger<UpdateApiMethodDefinitionHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateApiMethodDefinitionCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Key);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}