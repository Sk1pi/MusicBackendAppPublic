using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Application.Common.Interfaces.Repositories;
using MusicBackendApp.Domain.Entites.Subscriptions;
using MusicBackendApp.Domain.Entites.Subscriptions.SunInterfaces;
using MusicBackendApp.Domain.Shared;
using MusicBackendApp.Infrastructure.DataBase;

namespace MusicBackendApp.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly DbContextAccess _contextAccess;
    private ISubscriptionRepository _subscriptionRepositoryImplementation;

    public SubscriptionRepository(DbContextAccess contextAccess)
    {
        _contextAccess = contextAccess;
    }

    /*public async Task<Result<IUserSubscription, Error>> GetByIdAsync(UserId subscriptionId)
    {
        var user = await _contextAccess.Users
            .FirstOrDefaultAsync(x => x.Id == subscriptionId);
        if(user == null)
            return Result.Failure<IUserSubscription, Error>(Errors.General.NotFound(subscriptionId.ToString()));
            
        return Result.Success<IUserSubscription, Error>(user);
    }

    public async Task UpdateAsync(IUserSubscription subscription)
    {
        await _contextAccess.SaveChangesAsync();
    }*/


    public async Task<Result<IUserSubscription, Error>> GetByIdAsync(Guid subscriptionId)
    {
        var familySub = await _contextAccess.FamilySubscriptions.FirstOrDefaultAsync(s => s.MainAccountHolderId == subscriptionId);
        if (familySub != null) return Result.Success<IUserSubscription, Error>(familySub);
        
        var premiumSub = await _contextAccess.PremiumSubscriptions.FirstOrDefaultAsync(s => s.MainAccountId == subscriptionId);
        if (premiumSub != null) return Result.Success<IUserSubscription, Error>(premiumSub);
        
        var studentSub = await _contextAccess.StudentSubscriptions.FirstOrDefaultAsync(s => s.MainAccountId == subscriptionId);
        if (studentSub != null) return Result.Success<IUserSubscription, Error>(studentSub);

        return Result.Failure<IUserSubscription, Error>(
            Errors.General.NotFound($"Subscription with ID {subscriptionId}"));
    }

    public async Task UpdateAsync(IUserSubscription subscription)
    {
        if (subscription is FamilySubscription familySubscription)
            _contextAccess.FamilySubscriptions.Update(familySubscription);
        
        if(subscription is PremiumSubscription premiumSubscription)
            _contextAccess.PremiumSubscriptions.Update(premiumSubscription);
        
        if(subscription is StudentSubscription studentSubscription)
            _contextAccess.StudentSubscriptions.Update(studentSubscription);
        
        await _contextAccess.SaveChangesAsync();
    }

    public async Task AddAsync(IUserSubscription newSubscription)
    {
        if (newSubscription is FamilySubscription familySubscription)
            await _contextAccess.FamilySubscriptions.AddAsync(familySubscription);
        
        if(newSubscription is PremiumSubscription premiumSubscription) 
            await _contextAccess.PremiumSubscriptions.AddAsync(premiumSubscription);
        
        if(newSubscription is StudentSubscription studentSubscription)
            await _contextAccess.StudentSubscriptions.AddAsync(studentSubscription);
        
        await _contextAccess.SaveChangesAsync();
    }
}