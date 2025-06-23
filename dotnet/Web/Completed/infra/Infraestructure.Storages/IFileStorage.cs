namespace Infraestructure.Storages;

public interface IFileStorage
{
    Task<Stream> DownloadFileAsync(
        string path,
        string name,
        CancellationToken cancellationToken = default
    );

    Task UploadFileAsync(
        Stream content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    );
}
