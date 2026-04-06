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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUser;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUser
{
    public class DeleteItemAspNetUserCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }

        public DeleteItemAspNetUserCommand(string id)
        {
            this.Id = id;
        }

        public class DeleteItemAspNetUserHandler : IRequestHandler<DeleteItemAspNetUserCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetUserHandler> _logger;
            private readonly IAspNetUserRepository _repository;
            public DeleteItemAspNetUserHandler(ILogger<DeleteItemAspNetUserHandler> logger, IAspNetUserRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetUserCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.Id);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}