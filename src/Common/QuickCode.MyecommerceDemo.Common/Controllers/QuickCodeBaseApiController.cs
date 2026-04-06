using Microsoft.AspNetCore.Mvc;
using QuickCode.MyecommerceDemo.Common.Filters;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.Common.Controllers;

[ApiExceptionFilter]
[ApiController]
[ApiKey]
[Route("api/[controller]")]
public class QuickCodeBaseApiController : ControllerBase
{
    protected IActionResult? ValidatePagination(int? page, int? size) =>
        page < ConfigurationConstants.MinPageNumber ? NotFound($"Page number must be greater than {ConfigurationConstants.MinPageNumber }") :
        size > ConfigurationConstants.MaxPageSize ? BadRequest($"Page size cannot exceed {ConfigurationConstants.MaxPageSize }") :
        null;
    
    protected IActionResult? HandleResponseError<T>(Response<T> response, ILogger logger, string entityName, string? key = null)
    {
        if (key != null && response.Code == 404)
        {
            var notFoundMessage = $"{key} not found in {entityName} Table"; 
            logger.LogWarning("Api Response Error: '{NotFoundMessage}''", notFoundMessage);
            return NotFound(notFoundMessage);
        }
        else if (response.Code == 400)
        {
            return BadRequest($"Update Error ({entityName}): Response Code: {response.Code}, Message: {response.Message}");
        }
        else if (response.Code != 0)
        {
            var errorMessage = $"Response Code: {response.Code}, Message: {response.Message}"; 
            logger.LogError("Api Response Error: '{ErrorMessage}''", errorMessage);
            return BadRequest(errorMessage);
        }
        
        return null; 
    }
}