using CSharpFunctionalExtensions;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Common.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Task<Result<IUserSubscription, Error>> GetByIdAsync(Guid subscriptionId);
    Task UpdateAsync(IUserSubscription subscription);

    Task AddAsync(IUserSubscription subscription);
}