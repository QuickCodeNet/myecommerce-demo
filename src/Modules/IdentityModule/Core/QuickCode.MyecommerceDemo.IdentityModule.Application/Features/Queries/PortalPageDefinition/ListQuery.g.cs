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
    public class ListPortalPageDefinitionQuery : IRequest<Response<List<PortalPageDefinitionDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListPortalPageDefinitionQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListPortalPageDefinitionHandler : IRequestHandler<ListPortalPageDefinitionQuery, Response<List<PortalPageDefinitionDto>>>
        {
            private readonly ILogger<ListPortalPageDefinitionHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public ListPortalPageDefinitionHandler(ILogger<ListPortalPageDefinitionHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<PortalPageDefinitionDto>>> Handle(ListPortalPageDefinitionQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}