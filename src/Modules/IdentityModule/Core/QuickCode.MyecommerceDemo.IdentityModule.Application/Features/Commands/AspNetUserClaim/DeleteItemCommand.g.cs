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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserClaim
{
    public class DeleteItemAspNetUserClaimCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }

        public DeleteItemAspNetUserClaimCommand(int id)
        {
            this.Id = id;
        }

        public class DeleteItemAspNetUserClaimHandler : IRequestHandler<DeleteItemAspNetUserClaimCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetUserClaimHandler> _logger;
            private readonly IAspNetUserClaimRepository _repository;
            public DeleteItemAspNetUserClaimHandler(ILogger<DeleteItemAspNetUserClaimHandler> logger, IAspNetUserClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetUserClaimCommand request, CancellationToken cancellationToken)
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