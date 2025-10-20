using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;

namespace MusicBackendApp.Domain.Entites.Subscriptions;

public class FamilySubscription : IUserSubscription
{
    public Guid Id { get; } 
    public decimal BaseMonthlyPrice { get; }
    public decimal BaseYearlyPrice { get; } // Це ціна, яка вже включає будь-які знижки за річну оплату
    public decimal ActualPricePaid { get; } 
    public PaymentType PaymentFrequency { get; } 
    
    public FamilySubscription(Guid id,
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
    
    public Guid? MainAccountHolderId { get; private set; } // ID головного користувача

    public void AssignMainAccount(Guid userId)
    {
        if (MainAccountHolderId.HasValue)
        {
            throw new InvalidOperationException("Main account holder already assigned.");
        }
        MainAccountHolderId = userId;
        CanControlContent(userId);
    }
    
    public bool CanControlContent(Guid userId)
    {
        return MainAccountHolderId == userId;
    }
    
    public decimal GetMonthlyPrice() => BaseMonthlyPrice;
    public decimal GetYearlyPrice() => BaseYearlyPrice;
    public bool IsAdFree => true; // Преміум без реклами
    public int MaxOfflineDownloads => 1000; // Можна завантажити багато
    public int MaxConcurrentStreams => 3; // 3 одночасних стріми
    public int MaxAccounts => 2; // 1 основний акаунт
}