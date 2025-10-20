using AutoMapper;

namespace MusicBackendApp.Application.Common.Interfaces;

public interface IMapWith<T>
{
    void Mapping(Profile profile) => 
        profile.CreateMap(typeof(T), GetType());
}