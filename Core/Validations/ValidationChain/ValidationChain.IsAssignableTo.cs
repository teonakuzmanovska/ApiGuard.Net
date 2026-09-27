using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class ValidationChain
{
    public ValidationChain IsAssignableTo<T>(
        T? property,
        Type type,
        [CallerArgumentExpression(nameof(property))] string propertyName = "")
    {
        if (Error is not null)
        {
            return this;
        }

        if (property is null || !type.IsInstanceOfType(property))
        {
            Error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"{propertyName} must be assignable to {type.Name}."
            };
        }

        return this;
    }
}