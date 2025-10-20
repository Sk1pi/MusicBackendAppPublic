using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Subscriptions;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.AssignFamilySubscription;

public class AssignFamilySubscriptionAdminCommandHandler: IRequestHandler<AssignFamilySubscriptionAdminCommand, Result<Unit, Error>>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    // private readonly PaymentType _paymentType; // <--- ВИДАЛЕНО

    public AssignFamilySubscriptionAdminCommandHandler(
        ISubscriptionRepository subscriptionRepository) // <--- ВИПРАВЛЕНО КОНСТРУКТОР
    {
        _subscriptionRepository = subscriptionRepository;
        // _paymentType = paymentType; // <--- ВИДАЛЕНО
    }
    
    public async Task<Result<Unit, Error>> Handle(AssignFamilySubscriptionAdminCommand request, CancellationToken cancellationToken)
    {
        var subscriptionResult  = await _subscriptionRepository.GetByIdAsync(request.SubscriptionId);

        if (subscriptionResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(subscriptionResult.Error);
        }
        
        var subscription = subscriptionResult.Value;
        
        // Перевірити, чи це дійсно FamilySubscription
        if (!(subscription is FamilySubscription familySubscription))
        {
            return Result.Failure<Unit, Error>(Errors.AssignFamily.IsNotFamilySub(request.SubscriptionId.ToString()));
        }

        // --- ЦЕЙ БЛОК ЛОГІКИ ЩОДО ЦІНИ МАЄ БУТИ ВИДАЛЕНИЙ З ЦЬОГО ХЕНДЛЕРА ---
        // if (request.PaymentType == PaymentType.Monthly)
        //     subscription.GetMonthlyPrice();
        // else subscription.GetYearlyPrice();
        // ------------------------------------------------------------------
        
        familySubscription.AssignMainAccount(request.UserId);
        
        await _subscriptionRepository.UpdateAsync(familySubscription);
        
        return Result.Success<Unit, Error>(Unit.Value);
    }
}