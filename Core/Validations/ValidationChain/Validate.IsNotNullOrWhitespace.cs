using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsNotNullOrWhitespace(
        string? property,
        string propertyName)
    {
        if (_error is not null)
        {
            return this;
        }

        if (string.IsNullOrWhiteSpace(property))
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = "String property must not be null, empty or whitespace."
            };
        }

        return this;
    }
}