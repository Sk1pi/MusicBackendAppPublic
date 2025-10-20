using MusicBackendApp.Domain.Entites.Id_s;

namespace MusicBackendApp.Application.Common.SearchModels;

public class TrackSearchModel
{
    public string Id { get; set; } // ID треку
    public string Title { get; set; } // Назва треку
    public string ArtistName { get; set; } // Ім'я виконавця

    public ArtistId ArtistId { get; set; }
    // Можна додати інші поля, наприклад, "Summary", "CollaborationNote", "Lyrics" тощо
}