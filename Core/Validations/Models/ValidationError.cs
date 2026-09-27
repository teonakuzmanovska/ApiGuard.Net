namespace Core.Validations.Models;

public class ValidationError
{
    public required string PropertyName { get; init; }
    
    public required string ErrorMessage { get; init; }
}