using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;

namespace MusicBackendApp.Domain.Entites.Subscriptions;

public class PremiumSubscription : IUserSubscription
{
    public Guid Id { get; } 
    public decimal BaseMonthlyPrice { get; }
    public decimal BaseYearlyPrice { get; } 
    public decimal ActualPricePaid { get; } 
    public PaymentType PaymentFrequency { get; } 
    
    public PremiumSubscription(Guid id, 
        decimal baseMonthlyPrice,
        decimal baseYearlyPrice,
        PaymentType paymentFrequency)
    {
        Id = id;
        BaseMonthlyPrice = baseMonthlyPrice;
        BaseYearlyPrice = baseYearlyPrice;
        PaymentFrequency = paymentFrequency;
        ActualPricePaid = (paymentFrequency == PaymentType.Monthly) ? BaseMonthlyPrice : BaseYearlyPrice;
    }
    
    public decimal GetMonthlyPrice() => BaseMonthlyPrice;
    public decimal GetYearlyPrice() => BaseYearlyPrice;
    public bool IsAdFree => true; 
    public int MaxOfflineDownloads => 1000; 
    public int MaxConcurrentStreams => 3; 
    public int MaxAccounts => 1; 
    public Guid? MainAccountId { get; private set; } 
}