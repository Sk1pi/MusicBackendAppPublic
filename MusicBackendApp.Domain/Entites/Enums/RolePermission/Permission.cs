namespace MusicBackendApp.Domain.Entites.Enums.RolePermission;

public enum Permission
{
    // Дозволи для треків
    Tracks_Create,
    Tracks_Read,
    Tracks_Update,
    Tracks_Delete,

    // Дозволи для артистів
    Artists_Create,
    Artists_Read,
    Artists_Update,
    Artists_Delete,
    
    // Дозволи для підписок
    Subscriptions_Create,
    Subscriptions_Manage,
    Subscriptions_Cancel,
    
    Subscriptions_OfflineDownloads,
    Subscriptions_AdFreeListening,
    
    Users_Create,
    Users_Read,
    Users_Update,
    Users_Delete,
}