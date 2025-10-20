using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;

namespace MusicBackendApp.Application.Factories.UserSubscriptions;

public abstract class UserSubscriptionFactory
{
    // Фабричний метод, який створює об'єкт IUserSubscription
    // Він приймає обрану PaymentType, щоб абонемент знав, за якою системою його купили
    public abstract IUserSubscription CreateSubscription(PaymentType paymentType);
}