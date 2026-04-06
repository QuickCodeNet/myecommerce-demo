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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.Module;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.Module
{
    public class GetItemModuleQuery : IRequest<Response<ModuleDto>>
    {
        public string Name { get; set; }

        public GetItemModuleQuery(string name)
        {
            this.Name = name;
        }

        public class GetItemModuleHandler : IRequestHandler<GetItemModuleQuery, Response<ModuleDto>>
        {
            private readonly ILogger<GetItemModuleHandler> _logger;
            private readonly IModuleRepository _repository;
            public GetItemModuleHandler(ILogger<GetItemModuleHandler> logger, IModuleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ModuleDto>> Handle(GetItemModuleQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Name);
                return returnValue.ToResponse();
            }
        }
    }
}