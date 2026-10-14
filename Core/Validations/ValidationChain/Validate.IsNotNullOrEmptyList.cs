using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsNotNullOrEmptyList<T>(
        IEnumerable<T>? property,
        string propertyName)
    {
        if (_error is not null)
        {
            return this;
        }

        if (property is null || !property.Any())
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = "List property must not be null or empty."
            };
        }

        return this;
    }
}