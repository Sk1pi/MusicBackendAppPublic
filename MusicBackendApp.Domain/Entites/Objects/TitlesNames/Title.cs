using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites.Objects.TitlesNames;

public class Title
{
    public const int MaxLength = 30;
    private const int MinLength = 1;

    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Title, Error>(Errors.General.ValueIsRequired("Name"));
        }
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return Result.Failure<Title, Error>(Errors.General.ValueIsInvalid("Name"));
        }
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9\s.,!?'""-:]+$")) // Приклад для назви
        {
            return Result.Failure<Title, Error>(Errors.General.ValueIsInvalid("Title format"));
        }

        return new Title(value);
    }
    
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    
    public override string ToString() => Value;
    
    public static bool operator ==(Title? left, Title? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    public static bool operator !=(Title? left, Title? right) => !(left == right);
}