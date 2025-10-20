using FluentValidation;
using MusicBackendApp.Application.Common.Exception;
using MusicBackendApp.Domain.Entites.Objects.TitlesNames;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Queries.GetUserByUserName;

public class GetUserByIUserQueryValidator : AbstractValidator<GetUserByIUserQuery>
{
    public GetUserByIUserQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .NotEmpty().WithMessage(Errors.General.ValueIsRequired("Artist").Serialize())
            .MustBeValueObject(UserName.Create);
    }
}