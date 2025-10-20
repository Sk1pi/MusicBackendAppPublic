using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Entites.Subscriptions;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;

namespace MusicBackendApp.Application.Factories.UserSubscriptions;

public class PremiumSubscriptionFactory : UserSubscriptionFactory
{
    // Ціни можуть бути з конфігурації або константами
    private readonly decimal _monthlyPrice = 9.99m;
    private readonly decimal _yearlyPrice = 99.99m; // 9.99 * 10 = знижка 2 місяці

    public override IUserSubscription CreateSubscription(PaymentType paymentType)
    {
        // Створення нового ID для підписки
        var subscriptionId = Guid.NewGuid();
        return new PremiumSubscription(subscriptionId, _monthlyPrice, _yearlyPrice, paymentType);
    }
}