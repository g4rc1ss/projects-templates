namespace Infraestructure.Storages;

public class LocalStorage : IFileStorage
{
    public Task<Stream> DownloadFileAsync(
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult<Stream>(File.Open(path + name, FileMode.Open, FileAccess.ReadWrite));
    }

    public async Task UploadFileAsync(
        Stream content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        await using FileStream file = new(path + name, FileMode.Create, FileAccess.Write);
        await content.CopyToAsync(file, cancellationToken);
    }
}
