using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result<Guid, Error>>
{
    public string Name { get; init; }
    public string? Password { get; init; } 
    public string? Email { get; init; } 
    public decimal LikedTracks { get; init; }
    public int Subs { get; init; }
}