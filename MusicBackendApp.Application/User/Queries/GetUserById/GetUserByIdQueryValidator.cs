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
    }
}