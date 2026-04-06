using QuickCode.MyecommerceDemo.Common.Mediator;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.TableComboboxSetting;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.TableComboboxSetting;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Api.Controllers
{
    public partial class TableComboboxSettingsController : QuickCodeBaseApiController
    {
        private readonly IMediator mediator;
        private readonly ILogger<TableComboboxSettingsController> logger;
        private readonly IServiceProvider serviceProvider;
        public TableComboboxSettingsController(IMediator mediator, IServiceProvider serviceProvider, ILogger<TableComboboxSettingsController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TableComboboxSettingDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await mediator.Send(new ListTableComboboxSettingQuery(page, size));
            if (HandleResponseError(response, logger, "TableComboboxSetting", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await mediator.Send(new TotalCountTableComboboxSettingQuery());
            if (HandleResponseError(response, logger, "TableComboboxSetting") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{tableName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TableComboboxSettingDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(string tableName)
        {
            var response = await mediator.Send(new GetItemTableComboboxSettingQuery(tableName));
            if (HandleResponseError(response, logger, "TableComboboxSetting", $"TableName: '{tableName}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TableComboboxSettingDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(TableComboboxSettingDto model)
        {
            var response = await mediator.Send(new InsertTableComboboxSettingCommand(model));
            if (HandleResponseError(response, logger, "TableComboboxSetting") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { tableName = response.Value.TableName }, response.Value);
        }

        [HttpPut("{tableName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(string tableName, TableComboboxSettingDto model)
        {
            if (!(model.TableName == tableName))
            {
                return BadRequest($"TableName: '{tableName}' must be equal to model.TableName: '{model.TableName}'");
            }

            var response = await mediator.Send(new UpdateTableComboboxSettingCommand(tableName, model));
            if (HandleResponseError(response, logger, "TableComboboxSetting", $"TableName: '{tableName}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpDelete("{tableName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(string tableName)
        {
            var response = await mediator.Send(new DeleteItemTableComboboxSettingCommand(tableName));
            if (HandleResponseError(response, logger, "TableComboboxSetting", $"TableName: '{tableName}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}