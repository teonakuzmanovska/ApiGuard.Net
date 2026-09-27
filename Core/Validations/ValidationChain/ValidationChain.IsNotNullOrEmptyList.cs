using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class ValidationChain
{
    public ValidationChain IsNotNullOrEmptyList<T>(
        IEnumerable<T>? property,
        [CallerArgumentExpression(nameof(property))] string propertyName = "")
    {
        if (Error is not null)
        {
            return this;
        }

        if (property is null || !property.Any())
        {
            Error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"{propertyName} must not be null or empty."
            };
        }

        return this;
    }
}