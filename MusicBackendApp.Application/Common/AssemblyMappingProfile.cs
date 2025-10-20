using System.Reflection;
using AutoMapper;
using MusicBackendApp.Application.Common.Interfaces;

namespace MusicBackendApp.Application.Common;

//Цей клас є автоматизованим налаштувальником AutoMapper,
//який використовує рефлексію для пошуку та реєстрації всіх мапінгів у збірці.
public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(Assembly assembly)
    {
        ApplyMappingsFromAssembly(assembly);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()//Отримує всі публічні типи (класи, інтерфейси тощо) з наданої збірки.
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
            //Він шукає тільки ті класи, які реалізують інтерфейс IMapWith<T>.
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod("Mapping");
            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}