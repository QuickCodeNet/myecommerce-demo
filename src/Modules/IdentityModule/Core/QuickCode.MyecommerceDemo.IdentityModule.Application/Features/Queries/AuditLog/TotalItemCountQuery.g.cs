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
    public class TotalCountAuditLogQuery : IRequest<Response<int>>
    {
        public TotalCountAuditLogQuery()
        {
        }

        public class TotalCountAuditLogHandler : IRequestHandler<TotalCountAuditLogQuery, Response<int>>
        {
            private readonly ILogger<TotalCountAuditLogHandler> _logger;
            private readonly IAuditLogRepository _repository;
            public TotalCountAuditLogHandler(ILogger<TotalCountAuditLogHandler> logger, IAuditLogRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountAuditLogQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}