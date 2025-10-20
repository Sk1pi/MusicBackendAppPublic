using MediatR;

namespace MusicBackendApp.Application.User.Queries.GetUserByUserName;

public class GetUserByIUserQuery : IRequest<UserListVm>
{
    public string Name { get; set; }
}