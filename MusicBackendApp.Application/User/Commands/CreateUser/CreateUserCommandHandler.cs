using CSharpFunctionalExtensions;
using MassTransit;
using MediatR;
using MusicBackendApp.Application.Common.Events.Created;
using MusicBackendApp.Application.Common.Interfaces;
using MusicBackendApp.Application.Common.Interfaces.Auth;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.CreateUser;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IDbContextAccess dbContextAccess,
    IPublishEndpoint publishEndpoint,
    IPasswordHasher passwordHasher)
    : IRequestHandler<CreateUserCommand, Result<Guid, Error>>
{
    public async Task<Result<Guid, Error>> Handle(CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var nameResult = UserName.Create(request.Name);
        if (nameResult.IsFailure) return Result.Failure<Guid, Error>(nameResult.Error);
        
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure) return Result.Failure<Guid, Error>(emailResult.Error);
        
        var hashedPassword = passwordHasher.Generate(request.Password);
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
            return Result.Failure<Guid, Error>(Errors.General.ValueIsInvalid("Password"));
        
        var existingUserNameResult = await userRepository.FindByEmailAsync(emailResult.Value);
        if (existingUserNameResult.IsSuccess)
        {
            return Result.Failure<Guid, Error>(Errors.Module.AlreadyExist($"Artist already has a track named '{emailResult.Value}'")); // <= Повернути Result.Failure
        }

        var userCreateResult =
            Domain.Entites.User.Create(
                nameResult.Value, 
                emailResult.Value, 
                hashedPassword,
                request.LikedTracks, 
                request.Subs);
        
        // Перевіряємо результат створення User Entity (якщо фабричний метод повертає Result)
        if (userCreateResult.IsFailure)
            return Result.Failure<Guid, Error>(userCreateResult.Error);
        
        var user = userCreateResult.Value; // Отримуємо створену сутність User
        
        await userRepository.AddAsync(user);
        await dbContextAccess.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new UserCreatedEvent
        {
            UserId = user.Id.Value,
            UserName = user.Name.Value
        });
        
        return Result.Success<Guid, Error>(user.Id.Value);
    }
}