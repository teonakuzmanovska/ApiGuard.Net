using Core.Validations.Models;

namespace Core.Validations.ValidationChain;

public partial class Validate
{
    public Validate IsSatisfied(
        Func<bool> condition,
        string propertyName,
        string errorMessage)
    {
        if (_error is not null)
        {
            return this;
        }

        if (!condition())
        {
            _error = new ValidationError
            {
                PropertyName = propertyName,
                ErrorMessage = errorMessage
            };
        }

        return this;
    }
}
