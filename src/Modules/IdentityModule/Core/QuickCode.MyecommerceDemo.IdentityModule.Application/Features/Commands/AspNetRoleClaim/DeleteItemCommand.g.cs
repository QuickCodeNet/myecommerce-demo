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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetRoleClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetRoleClaim
{
    public class DeleteItemAspNetRoleClaimCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public DeleteItemAspNetRoleClaimCommand(int id)
        {
            this.Id = id;
        }

        public class DeleteItemAspNetRoleClaimHandler : IRequestHandler<DeleteItemAspNetRoleClaimCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetRoleClaimHandler> _logger;
            private readonly IAspNetRoleClaimRepository _repository;
            public DeleteItemAspNetRoleClaimHandler(ILogger<DeleteItemAspNetRoleClaimHandler> logger, IAspNetRoleClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetRoleClaimCommand request, CancellationToken cancellationToken)
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