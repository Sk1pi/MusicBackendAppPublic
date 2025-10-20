using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.PurchaseSubscription;

public class PurchaseSubscriptionCommand : IRequest<Result<Unit, Error>>
{
    public SubscriptionType SubscriptionType { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid StudentCardId { get; set; }
    public Guid SubscriptionId { get; set; }
    public Guid FamilyUserId { get; set; }
    public Guid UserId { get; set; }
}