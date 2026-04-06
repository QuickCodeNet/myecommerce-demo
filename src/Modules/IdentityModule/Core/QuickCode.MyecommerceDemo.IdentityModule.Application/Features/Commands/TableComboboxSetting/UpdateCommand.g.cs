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
    public class UpdateTableComboboxSettingCommand : IRequest<Response<bool>>
    {
        public string TableName { get; set; }
        public TableComboboxSettingDto request { get; set; }

        public UpdateTableComboboxSettingCommand(string tableName, TableComboboxSettingDto request)
        {
            this.request = request;
            this.TableName = tableName;
        }

        public class UpdateTableComboboxSettingHandler : IRequestHandler<UpdateTableComboboxSettingCommand, Response<bool>>
        {
            private readonly ILogger<UpdateTableComboboxSettingHandler> _logger;
            private readonly ITableComboboxSettingRepository _repository;
            public UpdateTableComboboxSettingHandler(ILogger<UpdateTableComboboxSettingHandler> logger, ITableComboboxSettingRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateTableComboboxSettingCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.TableName);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}