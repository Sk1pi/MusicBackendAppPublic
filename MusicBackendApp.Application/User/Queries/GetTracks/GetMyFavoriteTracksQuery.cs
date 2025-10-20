using MediatR;
using MusicBackendApp.Application.Track.Queries.GetTrackByTitile;

namespace MusicBackendApp.Application.User.Queries.GetTracks;

public class GetMyFavoriteTracksQuery : IRequest<TrackListVm> { }