using CSharpFunctionalExtensions;
using MediatR;
using MusicBackendApp.Application.Common;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Application.Common.Interfaces.Services;
using MusicBackendApp.Application.Factories.UserSubscriptions;
using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.User.Commands.PurchaseSubscription;

public class PurchaseSubscriptionCommandHandler: IRequestHandler<PurchaseSubscriptionCommand, Result<Unit, Error>>
{
    private readonly IStudentCardVerificationService _studentCardVerificationService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly PremiumSubscriptionFactory _premiumFactory;
    private readonly StudentSubscriptionFactory _studentFactory;
    private readonly FamilySubscriptionFactory _familyFactory;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public PurchaseSubscriptionCommandHandler(
        IStudentCardVerificationService studentCardVerificationService,
        ISubscriptionRepository subscriptionRepository,
        PremiumSubscriptionFactory premiumFactory,
        StudentSubscriptionFactory studentFactory,
        FamilySubscriptionFactory familyFactory,
        IPaymentGatewayService paymentGatewayService)
    {
        _studentCardVerificationService = studentCardVerificationService;
        _subscriptionRepository = subscriptionRepository;
        _premiumFactory = premiumFactory;
        _studentFactory = studentFactory;
        _familyFactory = familyFactory;
        _paymentGatewayService = paymentGatewayService;
    }


    public async Task<Result<Unit, Error>> Handle(PurchaseSubscriptionCommand request, CancellationToken cancellationToken)
    {
        IUserSubscription newSubscription = null; // Змінна для нового абонемента
        
        // === Логіка для СТУДЕНТСЬКОГО абонемента ===
        if (request.SubscriptionType == SubscriptionType.Student)
        {
            var verificationResult = await _studentCardVerificationService.VerifyStudentCardAsync(request.StudentCardId.ToString());

            if (verificationResult.IsFailure)
                return Result.Failure<Unit, Error>(verificationResult.Error);
            
            if (!verificationResult.Value) // Карта недійсна
                return Result.Failure<Unit, Error>(Errors.Subscription.InvalidStudentCard()); // <--- ВИПРАВЛЕНО ПОМИЛКУ

            // Створюємо СТУДЕНТСЬКИЙ абонемент за допомогою фабрики
            // Припустимо, фабрики отримують ціну в конструкторі, або тут можна передавати
            newSubscription = _studentFactory.CreateSubscription(request.PaymentType); // <--- Фабрика створює об'єкт з PaymentType
        }
        // === Логіка для ПРЕМІУМ абонемента ===
        else if (request.SubscriptionType == SubscriptionType.Premium)
        {
            // Створюємо ПРЕМІУМ абонемент за допомогою фабрики
            newSubscription = _premiumFactory.CreateSubscription(request.PaymentType); // <--- Фабрика створює об'єкт з PaymentType
        }
        // === Логіка для СІМЕЙНОГО абонемента ===
        else if (request.SubscriptionType == SubscriptionType.Family)
        { 
            // Створюємо СІМЕЙНИЙ абонемент за допомогою фабрики
            newSubscription = _familyFactory.CreateSubscription(request.PaymentType); // <--- Фабрика створює об'єкт з PaymentType
        }
        else
        {
            return Result.Failure<Unit, Error>(Errors.Subscription.InvalidSubscriptionType()); // Якщо тип не розпізнано
        }
        
        // Перевіряємо, чи абонемент був створений (якщо тип не розпізнано)
        if (newSubscription == null)
        {
            return Result.Failure<Unit, Error>(Errors.General.ValueIsInvalid("Subscription creation failed"));
        }
        
        decimal priceToCharge = newSubscription.ActualPricePaid;
        var paymentResult = await _paymentGatewayService.ChargeAsync(request.UserId, priceToCharge, newSubscription.PaymentFrequency);
        if (paymentResult.IsFailure) return Result.Failure<Unit, Error>(paymentResult.Error);
        
        // === Загальна логіка: ДОДАЄМО НОВИЙ абонемент у репозиторій ===
        await _subscriptionRepository.AddAsync(newSubscription); // <--- МАЄ БУТИ AddAsync, НЕ UpdateAsync!
        // Тобі потрібно буде додати AddAsync до ISubscriptionRepository

        // ... Можливо, тут відправити подію через MassTransit: SubscriptionPurchasedEvent ...

        return Result.Success<Unit, Error>(Unit.Value);
    }
}