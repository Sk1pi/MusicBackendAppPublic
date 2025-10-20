using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Domain.Entites.Objects.TitlesNames;

public class ArtistName
{
    public const int MaxLength = 30;
    private const int MinLength = 2;

    public string Value { get; }

    public ArtistName(string value)
    {
        Value = value;
    }

    public static Result<ArtistName, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<ArtistName, Error>(Errors.General.ValueIsRequired("Name"));
        }
        if (value.Length < MinLength || value.Length > MaxLength)
        {
            return Result.Failure<ArtistName, Error>(Errors.General.ValueIsInvalid("Name"));
        }

        if (!Regex.IsMatch(value, @"^[a-zA-Z\s]+$"))
        {
            return Result.Failure<ArtistName, Error>(Errors.General.ValueIsInvalid("Name format"));
        }

        return new ArtistName(value);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType()) return false;
        ArtistName other = (ArtistName)obj;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase); 
    }
    
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    
    public static bool operator ==(ArtistName? left, ArtistName? right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        return left.Equals(right);
    }
    public static bool operator !=(ArtistName? left, ArtistName? right) => !(left == right);

    public override string ToString() => Value;
}