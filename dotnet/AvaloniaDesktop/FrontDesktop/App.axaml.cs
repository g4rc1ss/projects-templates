using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using FrontDesktop.ViewModels;
using FrontDesktop.Views;
#if (!SqlDatabase)
using Infraestructure.Database;
#endif
#if (!UseMemoryEvents)
using Infraestructure.Events;
#endif
#if (!StorageNone)
using Infraestructure.Storages;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FrontDesktop;

public class App : Application
{
    private Task? _host;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.Configuration.AddUserSecrets<App>();

        builder.Services.AddTransient<MainViewModel>();

#if (!DatabaseNone)
        builder.AddDatabaseConfig();
#endif
#if (!EventBusNone)
        builder.AddEventsServices();
#endif
#if (!StorageNone)
        builder.AddStorages();
#endif

        IHost app = builder.Build();
        MainViewModel mainViewModel = app.Services.GetRequiredService<MainViewModel>();

        _host = app.RunAsync();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow { DataContext = mainViewModel };
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                singleViewPlatform.MainView = new MainView { DataContext = mainViewModel };
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove = BindingPlugins
            .DataValidators.OfType<DataAnnotationsValidationPlugin>()
            .ToArray();

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin? plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}