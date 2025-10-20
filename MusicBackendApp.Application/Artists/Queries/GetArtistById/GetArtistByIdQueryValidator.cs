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
    }
}