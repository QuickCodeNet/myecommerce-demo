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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalPageDefinition;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalPageDefinition
{
    public class DeletePortalPageDefinitionCommand : IRequest<Response<bool>>
    {
        public PortalPageDefinitionDto request { get; set; }

        public DeletePortalPageDefinitionCommand(PortalPageDefinitionDto request)
        {
            this.request = request;
        }

        public class DeletePortalPageDefinitionHandler : IRequestHandler<DeletePortalPageDefinitionCommand, Response<bool>>
        {
            private readonly ILogger<DeletePortalPageDefinitionHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public DeletePortalPageDefinitionHandler(ILogger<DeletePortalPageDefinitionHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeletePortalPageDefinitionCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}