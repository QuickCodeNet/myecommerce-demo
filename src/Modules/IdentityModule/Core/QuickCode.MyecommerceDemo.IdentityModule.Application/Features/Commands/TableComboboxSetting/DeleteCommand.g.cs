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
    public class DeleteTableComboboxSettingCommand : IRequest<Response<bool>>
    {
        public TableComboboxSettingDto request { get; set; }

        public DeleteTableComboboxSettingCommand(TableComboboxSettingDto request)
        {
            this.request = request;
        }

        public class DeleteTableComboboxSettingHandler : IRequestHandler<DeleteTableComboboxSettingCommand, Response<bool>>
        {
            private readonly ILogger<DeleteTableComboboxSettingHandler> _logger;
            private readonly ITableComboboxSettingRepository _repository;
            public DeleteTableComboboxSettingHandler(ILogger<DeleteTableComboboxSettingHandler> logger, ITableComboboxSettingRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteTableComboboxSettingCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}