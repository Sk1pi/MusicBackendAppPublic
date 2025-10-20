using FluentValidation;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(UserName.Create);
        RuleFor(x => x.Email)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(Email.Create);
        RuleFor(x => x.Password)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(Password.Create);
    }
}