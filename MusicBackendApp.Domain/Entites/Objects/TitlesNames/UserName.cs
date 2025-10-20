using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites.Objects.TitlesNames;

public class UserName
{
    public const int MaxLength = 30;
    private const int MinLength = 2;

    public string Value { get; }

    private UserName(string value)
    {
        Value = value;
    }

    public static Result<UserName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<UserName, Error>(Errors.General.ValueIsRequired("Name"));
        }
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return Result.Failure<UserName, Error>(Errors.General.ValueIsInvalid("Name"));
        }
        if (!Regex.IsMatch(value, @"^[a-zA-Z\s]+$"))
        {
            return Result.Failure<UserName, Error>(Errors.General.ValueIsInvalid("User Name format"));
        }

        return new UserName(value);
    }
    
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    
    public static bool operator ==(UserName? left, UserName? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    public static bool operator !=(UserName? left, UserName? right) => !(left == right);

    public override string ToString() => Value;
}