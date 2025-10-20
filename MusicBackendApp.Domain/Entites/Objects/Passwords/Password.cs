using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites.Objects.Passwords;

public class Password
{
    public const int MaxLength = 30;
    private const int MinLength = 8;

    public string Value { get; }

    public Password(string value)
    {
        Value = value;
    }

    public static Result<Password, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Password, Error>(Errors.General.ValueIsRequired("Password"));
        }
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return Result.Failure<Password, Error>(Errors.General.ValueIsInvalid("Password"));
        } 

        if (!Regex.IsMatch(value, @"[A-Z]"))
        {
            return Result.Failure<Password, Error>(Errors.General.ValueIsInvalid("Password requires at least one uppercase letter."));
        }
        if (!Regex.IsMatch(value, @"[a-z]"))
        {
            return Result.Failure<Password, Error>(Errors.General.ValueIsInvalid("Password requires at least one lowercase letter."));
        }
        if (!Regex.IsMatch(value, @"[0-9]"))
        {
            return Result.Failure<Password, Error>(Errors.General.ValueIsInvalid("Password requires at least one digit."));
        }
        if (!Regex.IsMatch(value, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
        {
             return Result.Failure<Password, Error>(Errors.General.ValueIsInvalid("Password requires at least one special character."));
        }

        return new Password(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        Password other = (Password)obj;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase); 
    }

    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    
    public static bool operator ==(Password? left, Password? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    public static bool operator !=(Password? left, Password? right) => !(left == right);

    public override string ToString() => Value;
}