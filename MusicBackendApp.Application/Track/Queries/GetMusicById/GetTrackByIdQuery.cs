using MediatR;

namespace MusicBackendApp.Application.Track.Queries.GetMusicById;

public class GetTrackByIdQuery : IRequest<TrackIdVm>
{
    //public Guid UserId { get; set; }
    public Guid Id { get; set; }
}