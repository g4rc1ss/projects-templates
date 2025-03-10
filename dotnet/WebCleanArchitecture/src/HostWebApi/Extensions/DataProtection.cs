using Microsoft.AspNetCore.DataProtection;

namespace HostWebApi.Extensions;

internal static class DataProtection
{
    internal static void ConfigureDataProtectionProvider(this IServiceCollection services,
        IConfiguration configuration)
    {
        // string? keysFolder = configuration["keysFolder"]!;
        services.AddDataProtection()
            // .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName("Template.Host");
    }
}