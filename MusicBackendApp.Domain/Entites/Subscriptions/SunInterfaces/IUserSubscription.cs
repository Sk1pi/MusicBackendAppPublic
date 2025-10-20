using MusicBackendApp.Domain.Entites.Enums.UserSub;

namespace MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;

public interface IUserSubscription
{
    decimal GetYearlyPrice();
    decimal GetMonthlyPrice();
    bool IsAdFree { get; } // Чи немає реклами
    int MaxOfflineDownloads { get; } // Скільки пісень можна завантажити
    int MaxConcurrentStreams { get; } // Скільки одночасних стрімів
    int MaxAccounts { get; } // Макс. кількість акаунтів (для сімейного)
    decimal ActualPricePaid { get; } 
    PaymentType PaymentFrequency { get; } 
    Guid Id { get; } 
}