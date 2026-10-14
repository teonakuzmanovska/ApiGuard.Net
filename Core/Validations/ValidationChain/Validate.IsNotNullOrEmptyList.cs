using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsNotNullOrEmptyList<T>(
        IEnumerable<T>? property,
        string propertyName)
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
                ErrorMessage = "List property must not be null or empty."
            };
        }

        return this;
    }
}