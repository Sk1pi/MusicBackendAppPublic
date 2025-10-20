using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MusicBackendApp.Application.Common.Parameters;
using MusicBackendApp.Domain.Entites;

namespace MusicBackendApp.Infrastructure;

public static class QueryableExtensions
{
    public static IQueryable<Track> SortByTrack(
        this IQueryable<Track> query,
        SortParams sortParams,
        Expression<Func<Track, object>>? defaultOrderBy)
    {
        
        if (string.IsNullOrEmpty(sortParams.OrderBy))
            return query.OrderBy(defaultOrderBy!); 

        if(sortParams.Veriable == SortVeriable.Track)
            return query.OrderByDescending(GetTrackSelector(sortParams.OrderBy));
        
        return query.OrderBy(GetTrackSelector(sortParams.OrderBy));
    }
    
    private static Expression<Func<Track, object>> GetTrackSelector(string sortParamsOrderBy)
    {
        if(string.IsNullOrEmpty(sortParamsOrderBy))
            return p => p.Title;

        return sortParamsOrderBy switch
        {
            nameof(Track.Author) => p => p.Author,
            nameof(Track.CollaborationNote) => p => p.CollaborationNote!,
            _ => p => p.Title,
        };
    }

    public static IQueryable<Artist> SortByArtist(
        this IQueryable<Artist> query,
        SortParams sortParams,
        Expression<Func<Artist, object>>? defaultOrderBy)
    {
        if (string.IsNullOrEmpty(sortParams.OrderBy))
            return query.OrderBy(defaultOrderBy!);
        
        if(sortParams.Veriable == SortVeriable.Track)
            return query.OrderByDescending(GetArtistSelector(sortParams.OrderBy));
        
        return query.OrderBy(GetArtistSelector(sortParams.OrderBy));
    }

    private static Expression<Func<Artist, object>> GetArtistSelector(string sortParamsOrderBy)
    {
        if(string.IsNullOrEmpty(sortParamsOrderBy))
            return p => p.ArtistName;

        return sortParamsOrderBy switch
        {
            nameof(Artist.AuthoredTracks) => p => p.AuthoredTracks,
            _ => p => p.ArtistName,
        };
    }

    public static IQueryable<User> SortByUser(
        this IQueryable<User> query,
        SortParams sortParams,
        Expression<Func<User, object>>? defaultOrderBy)
    {
        if (string.IsNullOrEmpty(sortParams.OrderBy))
            return query.OrderBy(defaultOrderBy!);
        
        if(sortParams.Veriable == SortVeriable.Track)
            return query.OrderByDescending(GetUserSelector(sortParams.OrderBy));
        
        return query.OrderBy(GetUserSelector(sortParams.OrderBy));
    }
    
    private static Expression<Func<User, object>> GetUserSelector(string sortParamsOrderBy)
    {
        if(string.IsNullOrEmpty(sortParamsOrderBy))
            return p => p.Name;

        return sortParamsOrderBy switch
        {
            nameof(User.Name) => p => p.Name
        };
    }
    
    public static async Task<PagedResult<T>> ToPagedAsync<T>(this IQueryable<T> query, 
        PageParams pageParams)
    {
        var count = await query.CountAsync();
        if(count == 0)
            return new PagedResult<T>(new List<T>(), 0);
        
        var page = pageParams.Page ?? 1;
        var pageSize = pageParams.PageSize ?? 10;
        
        var skip = (page - 1) * pageSize;
        var result = await query.Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        return new PagedResult<T>(result, count);
    }
}