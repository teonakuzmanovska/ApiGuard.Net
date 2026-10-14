using Core.Validations.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Validations.Extensions;

public static class ValidationErrorExtensions
{
    public static BadRequestObjectResult ToBadRequest(this ValidationError error)
    {
        return new BadRequestObjectResult(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = error.ErrorMessage,
            Extensions = { ["propertyName"] = error.PropertyName }
        });
    }
}