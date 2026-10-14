using Core.Validations.Models;

namespace Core.Validations.Exceptions;

public class ApiValidationException(ValidationError error) : Exception(error.ErrorMessage)
{
    public string PropertyName { get; } = error.PropertyName;
    public string ErrorMessage { get; } = error.ErrorMessage;
}