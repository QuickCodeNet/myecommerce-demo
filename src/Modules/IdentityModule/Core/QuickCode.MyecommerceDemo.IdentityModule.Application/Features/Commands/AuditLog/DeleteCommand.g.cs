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
    public class DeleteAuditLogCommand : IRequest<Response<bool>>
    {
        public AuditLogDto request { get; set; }

        public DeleteAuditLogCommand(AuditLogDto request)
        {
            this.request = request;
        }

        public class DeleteAuditLogHandler : IRequestHandler<DeleteAuditLogCommand, Response<bool>>
        {
            private readonly ILogger<DeleteAuditLogHandler> _logger;
            private readonly IAuditLogRepository _repository;
            public DeleteAuditLogHandler(ILogger<DeleteAuditLogHandler> logger, IAuditLogRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteAuditLogCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}