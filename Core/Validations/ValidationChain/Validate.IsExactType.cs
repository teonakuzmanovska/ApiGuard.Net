using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsExactType<T>(
        T property,
        Type type,
        string propertyName) where T : notnull
    {
        if (_error is not null)
        {
            return this;
        }

        if (property.GetType() != type)
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"Property must be exactly of type {type.Name}."
            };
        }

        return this;
    }
}