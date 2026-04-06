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
    public class DeleteItemApiMethodDefinitionCommand : IRequest<Response<bool>>
    {
        public string Key { get; set; }

        public DeleteItemApiMethodDefinitionCommand(string key)
        {
            this.Key = key;
        }

        public class DeleteItemApiMethodDefinitionHandler : IRequestHandler<DeleteItemApiMethodDefinitionCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemApiMethodDefinitionHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public DeleteItemApiMethodDefinitionHandler(ILogger<DeleteItemApiMethodDefinitionHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemApiMethodDefinitionCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.Key);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}