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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.Model;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.Model
{
    public class UpdateModelCommand : IRequest<Response<bool>>
    {
        public string Name { get; set; }
        public string ModuleName { get; set; }
        public ModelDto request { get; set; }

        public UpdateModelCommand(string name, string moduleName, ModelDto request)
        {
            this.request = request;
            this.Name = name;
            this.ModuleName = moduleName;
        }

        public class UpdateModelHandler : IRequestHandler<UpdateModelCommand, Response<bool>>
        {
            private readonly ILogger<UpdateModelHandler> _logger;
            private readonly IModelRepository _repository;
            public UpdateModelHandler(ILogger<UpdateModelHandler> logger, IModelRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateModelCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Name, request.ModuleName);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}