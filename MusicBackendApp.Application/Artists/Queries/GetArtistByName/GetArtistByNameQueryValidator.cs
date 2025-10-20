using FluentValidation;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Artists.Queries.GetArtistByName;

public class GetArtistByNameQueryValidator : AbstractValidator<GetArtistByNameQuery>
{
    public GetArtistByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(ArtistName.Create);
    }
}