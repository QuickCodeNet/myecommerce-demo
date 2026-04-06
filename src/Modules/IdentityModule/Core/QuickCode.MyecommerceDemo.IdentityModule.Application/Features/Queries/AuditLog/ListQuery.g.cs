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
    public class ListAuditLogQuery : IRequest<Response<List<AuditLogDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAuditLogQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAuditLogHandler : IRequestHandler<ListAuditLogQuery, Response<List<AuditLogDto>>>
        {
            private readonly ILogger<ListAuditLogHandler> _logger;
            private readonly IAuditLogRepository _repository;
            public ListAuditLogHandler(ILogger<ListAuditLogHandler> logger, IAuditLogRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AuditLogDto>>> Handle(ListAuditLogQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}