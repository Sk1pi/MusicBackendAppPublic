using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using MusicBackendApp.Application.Common.Interfaces.Services;
using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly ILogger<PaymentGatewayService> _logger;
    private readonly Random _random = new Random(); 

    public PaymentGatewayService(ILogger<PaymentGatewayService> logger)
    {
        _logger = logger;
    }

    public Task<Result<bool, Error>> ChargeAsync(Guid userId, decimal amount, PaymentType paymentFrequency)
    {
        _logger.LogInformation($"[PaymentGateway] Attempting to charge user {userId} for {amount} {paymentFrequency} subscription.");
        
        if (_random.Next(1, 100) > 90)
        {
            _logger.LogError($"[PaymentGateway] Payment failed for user {userId}. Amount: {amount}");
            return Task.FromResult(Result.Failure<bool, Error>(Errors.Payment.PaymentFailed("Payment gateway declined the transaction.")));
        }

        _logger.LogInformation($"[PaymentGateway] Payment successful for user {userId}. Amount: {amount}");
        return Task.FromResult(Result.Success<bool, Error>(true));
    }
}