using FluentValidation;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Queries.GetMusicById;

public class GetTrackByIdQueryValidator : AbstractValidator<GetTrackByIdQuery>
{
    public GetTrackByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize());
    }
}