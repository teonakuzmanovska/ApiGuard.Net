using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    private ValidationError? _error;

    public ValidationResult Result() => new() { Error = _error };
}