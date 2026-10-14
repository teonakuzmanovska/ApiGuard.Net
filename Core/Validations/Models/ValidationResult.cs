using System.Diagnostics.CodeAnalysis;

namespace Core.Validations.Models;

public class ValidationResult
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsValid => Error is null;

    public ValidationError? Error { get; init; }
}