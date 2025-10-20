using MediatR;

namespace MusicBackendApp.Application.User.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserIdVm>, IRequest<Domain.Entites.User>
{
    public Guid Id { get; set; }
}