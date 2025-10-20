using FluentValidation;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .NotNull().WithMessage(Errors.General.ValueIsRequired("User ID").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("User ID").Serialize());
        // Тепер, коли ArtistId.FromGuid повертає Result<ArtistId, Error>,
        // компілятор зможе вивести типи автоматично, і явне вказання generic параметрів може бути не потрібним.

    }
}