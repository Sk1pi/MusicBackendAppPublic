using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Enums.UserSub;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Services;

public interface IPaymentGatewayService
{
    // Метод для імітації зняття грошей
    Task<Result<bool, Error>> ChargeAsync(Guid userId, decimal amount, PaymentType paymentFrequency);
}