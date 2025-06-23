using Microsoft.AspNetCore.DataProtection;

namespace Template.HostWebApi.Extensions;

internal static class DataProtectionExtensions
{
    internal static void ConfigureDataProtectionProvider(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDataProtection()
            // .PersistKeysToFileSystem(new DirectoryInfo("folder"))
            .SetApplicationName("Template.Host");
    }
}
