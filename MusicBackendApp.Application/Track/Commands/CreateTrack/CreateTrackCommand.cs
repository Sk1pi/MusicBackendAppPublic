using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using MusicBackendApp.Domain.Shared;

namespace MusicBackendApp.Application.Track.Commands.CreateTrack;

public class CreateTrackCommand : IRequest<Result<Guid, Error>>
{
    public string Title { get; set; }
    public TimeSpan Duration { get; set; } // Можна отримувати з метаданих файлу
    public string? ArtistId { get; set; }
    public string FilePath { get; set; }
    public int Valume { get; set; }
    public string? collaborationNote { get; init; }
    public IFormFile AudioFile { get; set; }
}