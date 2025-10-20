using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Subscriptions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.AssignFamilySubscription;

public class AssignFamilySubscriptionAdminCommandHandler: IRequestHandler<AssignFamilySubscriptionAdminCommand, Result<Unit, Error>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public AssignFamilySubscriptionAdminCommandHandler(
        ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }
    
    public async Task<Result<Unit, Error>> Handle(AssignFamilySubscriptionAdminCommand request, CancellationToken cancellationToken)
    {
        var subscriptionResult  = await _subscriptionRepository.GetByIdAsync(request.SubscriptionId);

        if (subscriptionResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(subscriptionResult.Error);
        }
        
        var subscription = subscriptionResult.Value;
        
        if (!(subscription is FamilySubscription familySubscription))
        {
            return Result.Failure<Unit, Error>(Errors.AssignFamily.IsNotFamilySub(request.SubscriptionId.ToString()));
        }
        
        familySubscription.AssignMainAccount(request.UserId);
        
        await _subscriptionRepository.UpdateAsync(familySubscription);
        
        return Result.Success<Unit, Error>(Unit.Value);
    }
}