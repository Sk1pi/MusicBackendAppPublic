using FluentValidation;

namespace MusicBackendApp.Application.Track.Commands.CreateTrack;

public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
{
    public CreateTrackCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва треку є обов'язковою.")
            .MaximumLength(100).WithMessage("Назва треку занадто довга.");

        RuleFor(x => x.AudioFile)
            .NotNull().WithMessage("Аудіофайл є обов'язковим.");
    }
}