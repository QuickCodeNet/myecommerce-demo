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
    public class GetItemAuditLogQuery : IRequest<Response<AuditLogDto>>
    {
        public Guid Id { get; set; }

        public GetItemAuditLogQuery(Guid id)
        {
            this.Id = id;
        }

        public class GetItemAuditLogHandler : IRequestHandler<GetItemAuditLogQuery, Response<AuditLogDto>>
        {
            private readonly ILogger<GetItemAuditLogHandler> _logger;
            private readonly IAuditLogRepository _repository;
            public GetItemAuditLogHandler(ILogger<GetItemAuditLogHandler> logger, IAuditLogRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AuditLogDto>> Handle(GetItemAuditLogQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Id);
                return returnValue.ToResponse();
            }
        }
    }
}