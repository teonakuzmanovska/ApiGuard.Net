using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class ValidationChain
{
    public ValidationChain IsNotNull<T>(
        T? property,
        [CallerArgumentExpression(nameof(property))] string propertyName = "")
    {
        if (Error is not null)
        {
            return this;
        }

        if (property is null)
        {
            Error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"{propertyName} must not be null."
            };
        }

        return this;
    }
}