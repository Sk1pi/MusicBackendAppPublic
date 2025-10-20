using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Auth;

public interface IPasswordHasher
{
    string Generate(string? password);
    
    bool Verify(string password, string hashedPassword);
}