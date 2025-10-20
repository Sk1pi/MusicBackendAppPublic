using FluentValidation;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

public class GetTrackByTitileQueryValidator : AbstractValidator<GetTrackByTitileQuery>
{
    public GetTrackByTitileQueryValidator()
    {
        RuleFor(x => x.Title)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(Title.Create);
    }
}