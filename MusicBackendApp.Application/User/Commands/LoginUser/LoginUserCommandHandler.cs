using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicBackendApp.Application.Common.Events.Login;
using MusicBackendApp.Application.Common.Interfaces.Auth;
using MusicBackendApp.Application.Common.Interfaces.Jwt;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.LoginUser;

public class LoginUserCommandHandler 
    : IRequestHandler<LoginUserCommand, Result<AuthResponse, Error>>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository, 
        IConfiguration configuration,
        IPublishEndpoint publishEndpoint,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _publishEndpoint = publishEndpoint;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthResponse, Error>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Валідація та створення Email Value Object
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            // Повертаємо загальну помилку, щоб не розкривати, що саме не так (email чи пароль)
            return Result.Failure<AuthResponse, Error>(Errors.General.NotFound());
        }
        var email = emailResult.Value;

        // 2. Пошук користувача в репозиторії
        // Припускаємо, що FindByEmailAsync повертає Result<User, Error>
        var userResult = await _userRepository.FindByEmailAsync(email);
        if (userResult.IsFailure)
        {
            // Користувача з таким email не знайдено
            return Result.Failure<AuthResponse, Error>(Errors.General.NotFound());
        }
        var user = userResult.Value;

        // 3. ПРАВИЛЬНА ПЕРЕВІРКА ПАРОЛЯ за допомогою IPasswordHasher
        // Раніше було user.Password.Value != request.Password, але user.Password.Value - це вже хеш
        // Тому потрібно перевіряти сирий пароль (request.Password) проти хешованого (user.Password.Value)
        if (!_passwordHasher.Verify(request.Password, user.Password.Value))
        {
            return Result.Failure<AuthResponse, Error>(Errors.General.NotFound());
        }

        var tokenString = _jwtProvider.GenerateToken(user);
        
        // 5. Повернення успішної відповіді з токеном
        var authResponse = new AuthResponse { Token = tokenString };
        
        await _publishEndpoint.Publish(new UserLoginEvent
        {
            UserId = user.Id.Value,
            UserName = user.Name.Value
        });
        
        return Result.Success<AuthResponse, Error>(authResponse);
    }
}