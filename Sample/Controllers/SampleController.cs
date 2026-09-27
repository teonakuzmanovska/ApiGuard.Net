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
        return Ok(request);
    }
}