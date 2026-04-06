using QuickCode.MyecommerceDemo.IdentityModule.Application.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Api.Application.Features.Queries.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Api.Controllers
{
    public partial class ApiMethodDefinitionsController 
    {
	    [HttpGet("get-api-permissions/{permissionGroupName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiModulePermissions))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetApiPermissions(string permissionGroupName)
        {
            var response = await mediator.Send(new ApiMethodAccessGrantGetItemsQuery(permissionGroupName));
            return Ok(response.Value);
        }
        
        [HttpPost("update-api-permission")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateApiPermission(UpdateApiMethodAccessGrantRequest request)
        {
            var response = await mediator.Send(new UpdateApiMethodAccessGrantCommand(
                request.PermissionGroupName, 
                request.ApiMethodDefinitionKey,
                new ApiMethodAccessGrantDto()
                {
                    PermissionGroupName = request.PermissionGroupName,
                    ApiMethodDefinitionKey = request.ApiMethodDefinitionKey,
                    ModifiedBy = request.Value == 1 ? ModificationType.User : ModificationType.UserDisabled,
                    IsActive = request.Value == 1
                }));

            return Ok(response.Code == 0);
        }
    }
}

