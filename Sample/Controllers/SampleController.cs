using Core.Validations.ValidationChain;
using Microsoft.AspNetCore.Mvc;
using Sample.Requests;

namespace Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateEmployeeRequest request)
    {
        var validationResult = new Validate()
            .IsNotNullOrEmptyString(request.Name)
            .IsNotNull(request.Age)
            .IsExactType(request.CurrentDepartment, typeof(Department))
            .IsNotNullOrEmptyList(request.PastDepartments)
            .Result();

        if(validationResult.IsValid)
        {
            return Ok(request);
        }
        
        return BadRequest(validationResult);
    }
}