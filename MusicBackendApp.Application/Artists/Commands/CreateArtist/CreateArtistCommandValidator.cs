using FluentValidation;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Domain.Entites.Objects.Email;
using MusicBackendApp.Domain.Entites.Objects.Passwords;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Artists.Commands.CreateArtist;


public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    public CreateArtistCommandValidator()
    {
        RuleFor(x => x.ArtistName)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(ArtistName.Create);
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
/*Причина: Методу розширення MustBeValueObject потрібні
 два параметри типу (TValue та TError), щоб він міг визначити тип 
 Result<TValue, TError>. Ви передаєте лише фабричний метод.*/