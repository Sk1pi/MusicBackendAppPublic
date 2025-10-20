using MusicBackendApp.Domain.Entites.Enums.UserSub;

namespace MusicBackendApp.Contracts;

public class PurchaseSubscriptionRequest
{
    // ID користувача, який купує підписку.
    // У реальному додатку ви б отримували його з токена авторизації,
    // а не передавали в запиті. Для прикладу залишимо тут.
    public Guid UserId { get; set; } 
    
    public SubscriptionType SubscriptionType { get; set; } // Premium, Student, Family
    public PaymentType PaymentType { get; set; } // Monthly, Yearly
    public Guid? StudentCardId { get; set; } // Необов'язково, для StudentSubscription
    // Можливо, тут будуть дані платіжної картки або токен оплати
    // public string PaymentToken { get; set; } 
}