using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsExactType<T>(
        T property,
        Type type,
        string propertyName) where T : notnull
    {
        if (Error is not null)
        {
            return this;
        }

        if (property.GetType() != type)
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