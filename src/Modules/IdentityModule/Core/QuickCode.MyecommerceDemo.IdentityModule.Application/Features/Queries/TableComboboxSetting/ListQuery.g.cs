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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.TableComboboxSetting;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.TableComboboxSetting
{
    public class ListTableComboboxSettingQuery : IRequest<Response<List<TableComboboxSettingDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListTableComboboxSettingQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListTableComboboxSettingHandler : IRequestHandler<ListTableComboboxSettingQuery, Response<List<TableComboboxSettingDto>>>
        {
            private readonly ILogger<ListTableComboboxSettingHandler> _logger;
            private readonly ITableComboboxSettingRepository _repository;
            public ListTableComboboxSettingHandler(ILogger<ListTableComboboxSettingHandler> logger, ITableComboboxSettingRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<TableComboboxSettingDto>>> Handle(ListTableComboboxSettingQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}