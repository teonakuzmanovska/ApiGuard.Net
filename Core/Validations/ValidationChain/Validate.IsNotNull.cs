using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsNotNull<T>(
        T? property,
        string propertyName)
    {
        if (_error is not null)
        {
            return this;
        }

        if (property is null)
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = "Property must not be null."
            };
        }

        return this;
    }
}