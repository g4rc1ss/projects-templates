using System.Diagnostics;

namespace Infraestructure.Storages;

public class LocalStorage : IFileStorage
{
    public Task<Stream> DownloadFileAsync(
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        using ActivitySource? tracingConsumer = new(InfraStoragesExtensions.STORAGE_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Downloading file",
            ActivityKind.Producer
        );
        activity?.Start();
        
        return Task.FromResult<Stream>(File.Open(path + name, FileMode.Open, FileAccess.ReadWrite));
    }

    public async Task UploadFileAsync(
        Stream content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        using ActivitySource? tracingConsumer = new(InfraStoragesExtensions.STORAGE_TRACE);
        using Activity? activity = tracingConsumer.CreateActivity(
            $"Uploading file",
            ActivityKind.Producer
        );
        activity?.Start();

        await using FileStream file = new(path + name, FileMode.Create, FileAccess.Write);
        await content.CopyToAsync(file, cancellationToken);
    }
}
