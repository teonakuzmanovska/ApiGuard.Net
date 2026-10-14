using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsAssignableTo<T>(
        T property,
        Type type,
        string propertyName) where T : notnull
    {
        if (_error is not null)
        {
            return this;
        }

        if (!type.IsInstanceOfType(property))
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = $"{typeof(T)} is not assignable to {type.Name} type."
            };
        }

        return this;
    }
}