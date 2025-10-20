using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.LoginUser;

public class LoginUserCommand : IRequest<Result<AuthResponse, Error>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}