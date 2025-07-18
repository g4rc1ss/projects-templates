using Microsoft.AspNetCore.DataProtection;

namespace SimpleWeb.HostWebApi.Extensions;

internal static class DataProtectionExtensions
{
    internal static void ConfigureDataProtectionProvider(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddDataProtection()
            // .PersistKeysToFileSystem(new DirectoryInfo("folder"))
            .SetApplicationName(builder.Environment.ApplicationName);
    }
}
