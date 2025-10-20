using FluentValidation;

namespace MusicBackendApp.Application.Track.Commands.CreateTrack;

public class CreateTrackCommandValidator : AbstractValidator<CreateTrackCommand>
{
    public CreateTrackCommandValidator()
    {
        /*RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва треку є обов'язковою.")
            .MaximumLength(100).WithMessage("Назва треку занадто довга.");
                
        RuleFor(x => x.AudioFile)
            .NotNull().WithMessage("Аудіофайл є обов'язковим.")
            .Must(file => file.Length > 0).WithMessage("Файл не може бути порожнім.")
            .Must(file => file.ContentType.StartsWith("audio/")).WithMessage("Дозволено завантажувати лише аудіофайли.");     
        
        RuleFor(x => x.Title)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(Title.Create);*/
        
        // Залишаємо лише один, правильний набір правил для Title
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Назва треку є обов'язковою.")
            .MaximumLength(100).WithMessage("Назва треку занадто довга.");

        // Правило для завантаженого файлу
        RuleFor(x => x.AudioFile)
            .NotNull().WithMessage("Аудіофайл є обов'язковим.");
    }
}