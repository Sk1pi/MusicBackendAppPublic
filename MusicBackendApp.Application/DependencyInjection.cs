using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MusicBackendApp.Application.Common;

namespace MusicBackendApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    //IServiceCollection – це стандартний інтерфейс
    //ASP.NET Core для додавання сервісів у контейнер IoC.
    //Це дозволяє тобі викликати метод як services.AddApplicationServices() з Program.cs
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //Він вказує MediatR сканувати поточну збірку
        //(тобто MusicBackendApp.Application) на наявність усіх класів,
        //які реалізують IRequestHandler (ваші хендлери) та IRequestPreProcessor

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //Цей метод реєструє всі валідатори FluentValidation (AbstractValidator<T>),
        //які знаходяться в поточній збірці (MusicBackendApp.Application), у контейнері IoC
        
        //Це означає, що коли MediatR (через Pipeline Behavior)
        //знадобиться валідатор для CreateArtistCommand,
        //він зможе отримати екземпляр CreateArtistCommandValidator з контейнера.

        // Залишаємо лише один, правильний спосіб реєстрації AutoMapper
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        });

        return services;
    }
}