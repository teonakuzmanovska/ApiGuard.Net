using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class ValidationChain
{
    private ValidationError? Error { get; set; }

    public ValidationResult Result()
    {
        return new ValidationResult
        {
            Error = Error
        };
    }
}