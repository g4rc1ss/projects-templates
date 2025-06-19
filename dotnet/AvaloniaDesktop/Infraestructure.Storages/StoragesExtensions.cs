using Microsoft.Extensions.Hosting;
#if (UseLocalStorage)
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Infraestructure.Storages;

public static class StoragesExtensions
{
    public static void AddStorages(this IHostApplicationBuilder builder)
    {
#if (UseLocalStorage)
        builder.Services.AddSingleton<IFileStorage, LocalStorage>();
#endif
    }
}
