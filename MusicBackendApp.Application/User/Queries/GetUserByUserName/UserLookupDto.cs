using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;

namespace MusicBackendApp.Application.User.Queries.GetUserByUserName;

public class UserLookupDto : IMapWith<Domain.Entites.User>
{ 
    public Guid UserId { get; set; }
    public string? Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entites.User, UserLookupDto>()
            .ForMember(d => d.UserId, 
                opt => opt.MapFrom(s => s.Id.Value))
            .ForMember(d => d.Name, 
                opt => opt.MapFrom(s => s.Name.Value));
    }
}