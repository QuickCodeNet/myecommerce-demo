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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AuditLog;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AuditLog
{
    public class UpdateAuditLogCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public AuditLogDto request { get; set; }

        public UpdateAuditLogCommand(Guid id, AuditLogDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class UpdateAuditLogHandler : IRequestHandler<UpdateAuditLogCommand, Response<bool>>
        {
            private readonly ILogger<UpdateAuditLogHandler> _logger;
            private readonly IAuditLogRepository _repository;
            public UpdateAuditLogHandler(ILogger<UpdateAuditLogHandler> logger, IAuditLogRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateAuditLogCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}