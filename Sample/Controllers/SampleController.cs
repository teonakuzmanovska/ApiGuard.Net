using Core.Validations.Extensions;
using Core.Validations.ValidationChain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sample.Requests;

namespace Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpPost("validateRequest")]
    public IActionResult ValidateRequest(SampleEmployeeRequest request)
    {
        var validationResult = new Validate()
            .IsNotNullOrWhitespace(request.Name, nameof(request.Name))
            .IsNotNull(request.Age, nameof(request.Age))
            .IsExactType(request.CurrentDepartment, typeof(Department), nameof(request.CurrentDepartment))
            .IsNotNullOrEmptyList(request.PastDepartments, nameof(request.PastDepartments))
            .Result();

        if (!validationResult.IsValid)
        {
            return validationResult.Error.ToBadRequest();
        }
        
        return Ok();
    }
    
    [HttpGet("internalServerError")]
    public IActionResult Return500()
    {
        var internalError = "InternalError that must not be exposed.";
        
        throw new Exception(internalError);
    }
    
    [HttpGet("rateLimiting/policy1")]
    [EnableRateLimiting("policy1")]
    public IActionResult RateLimitingPolicy1()
    {
        return Ok();
    }

    [HttpGet("rateLimiting/policy2")]
    [EnableRateLimiting("policy2")]
    public IActionResult RateLimitingPolicy2()
    {
        return Ok();
    }
}