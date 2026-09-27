using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsNotNullOrEmptyString(
        string? property,
        [CallerArgumentExpression(nameof(property))] string propertyName = "")
    {
        if (Error is not null)
        {
            return this;
        }

        if (string.IsNullOrWhiteSpace(property))
        {
            Error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = "Property must not be null, empty or whitespace."
            };
        }

        return this;
    }
}