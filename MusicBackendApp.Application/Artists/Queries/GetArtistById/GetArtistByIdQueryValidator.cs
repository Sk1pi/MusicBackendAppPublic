using FluentValidation;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistById;

public class GetArtistByIdQueryValidator : AbstractValidator<GetArtistByIdQuery>
{
    public GetArtistByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist ID").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist ID").Serialize());
        // Тепер, коли ArtistId.FromGuid повертає Result<ArtistId, Error>,
        // компілятор зможе вивести типи автоматично, і явне вказання generic параметрів може бути не потрібним.
        // <= ЗАЛИШИТИ ТАК (без явних generic параметрів на MustBeValueObject)
        // Якщо все ще помилка, спробуйте знову з явними параметрами, як ми писали раніше:
        // .MustBeValueObject<GetArtistByIdQuery, Guid, ArtistId, Error>(ArtistId.FromGuid);
    }
}