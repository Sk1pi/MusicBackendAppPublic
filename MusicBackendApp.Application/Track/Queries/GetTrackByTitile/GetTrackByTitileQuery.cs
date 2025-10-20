using MediatR;

namespace MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

public class GetTrackByTitileQuery : IRequest<TrackListVm>
{
    public string Title { get; set; }
}