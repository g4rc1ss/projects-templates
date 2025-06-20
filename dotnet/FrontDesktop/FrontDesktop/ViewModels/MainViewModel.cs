﻿using CommunityToolkit.Mvvm.ComponentModel;
#if (UseLitedb)
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
#endif
#if (UseMemoryEvents || SqlDatabase || UseLocalStorage || NoSqlDatabase)
using Microsoft.Extensions.DependencyInjection;
#endif
#if SqlDatabase
using Infraestructure.Database.Entities;
using Infraestructure.Database.Repository;
#endif
#if (UseMemoryEvents)
using Infraestructure.Events.Handlers;
using Infraestructure.Events.Publisher;
#endif
#if UseLocalStorage
using Infraestructure.Storages;
#endif

namespace FrontDesktop.ViewModels;

public partial class MainViewModel(
#if (UseMemoryEvents || SqlDatabase || UseLocalStorage || NoSqlDatabase)
    IServiceProvider serviceProvider
#endif
) : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";

    public async Task ExecuteEventsAsync()
    {
#if (UseMemoryEvents)
        IEventNotificator publisher = serviceProvider.GetRequiredService<IEventNotificator>();
        await publisher.PublishAsync(new Request());
#endif
    }

    public async Task ExecuteCrudAsync()
    {
#if (SqlDatabase)
        IUserRepository userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        UserEntity user = new UserEntity { Name = "Nombre" };
        UserEntity userCreated = await userRepository.CreateAsync(user);
        user.Name = "actualizado";
        await userRepository.UpdateAsync(user);

        UserEntity? userGet = await userRepository.GetByIdAsync(userCreated.Id);
        Greeting = userGet?.Name ?? "Nombre";
#endif
#if (UseLitedb)
        ILitedbPoc poc = serviceProvider.GetRequiredService<ILitedbPoc>();
        LiteDbEntity entity = new() { Id = Guid.NewGuid().ToString() };
        await poc.CreateAsync(entity);
        entity.Property = "Campo agregado";
        await poc.UpdateAsync(entity);
        LiteDbEntity? pocGet = await poc.GetByIdAsync(entity.Id);
        Greeting = pocGet?.Property ?? "Nombre";
#endif
    }

    public async Task ExecuteStorageAsync()
    {
#if (UseLocalStorage)
        IFileStorage storage = serviceProvider.GetRequiredService<IFileStorage>();
        byte[] content = "Prueba de escritura de archivo de texto"u8.ToArray();
        await storage.UploadFileAsync(content, "./", "archivo_prueba.txt");
        Stream download = await storage.DownloadFileAsync("./", "archivo_prueba.txt");
        using StreamReader reader = new(download);
        Greeting = await reader.ReadToEndAsync();
#endif
    }
}
