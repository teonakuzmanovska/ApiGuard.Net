using System.Runtime.CompilerServices;
using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsExactType<T>(
        T? property,
        Type type,
        [CallerArgumentExpression(nameof(property))] string propertyName = "")
    {
        if (Error is not null)
        {
            return this;
        }

        if (property is null || property.GetType() != type)
        {
            Error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"Property must be exactly of type {type.Name}."
            };
        }

        return this;
    }
}