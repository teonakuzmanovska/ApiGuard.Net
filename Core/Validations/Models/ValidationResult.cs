namespace Core.Validations.Models;

public class ValidationResult
{
    public bool IsValid => Error is null;
    
    public ValidationError? Error { get; init; }
}