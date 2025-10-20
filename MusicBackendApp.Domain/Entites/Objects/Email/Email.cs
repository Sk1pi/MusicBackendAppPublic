using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites.Objects.Email;

public class Email
{
    public const int MaxLength = 100;
    private const int MinLength = 5;

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email, Error>(Errors.General.ValueIsRequired("Name"));
        }
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return Result.Failure<Email, Error>(Errors.General.ValueIsInvalid("Name"));
        }
        // Додана валідація формату Email
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return Result.Failure<Email, Error>(Errors.General.ValueIsInvalid("Email format"));
        }

        return new Email(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        Email other = (Email)obj;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public static bool operator ==(Email? left, Email? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    public static bool operator !=(Email? left, Email? right) => !(left == right);

    public override string ToString() => Value;
}