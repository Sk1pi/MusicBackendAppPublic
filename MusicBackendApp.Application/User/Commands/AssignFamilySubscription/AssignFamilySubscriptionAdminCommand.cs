using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.AssignFamilySubscription;

public class AssignFamilySubscriptionAdminCommand : IRequest<Result<Unit, Error>>
{
    public Guid SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public PaymentType PaymentType { get; set; }
}