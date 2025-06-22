namespace Infraestructure.Storages;

public static class FileStorageExtensions
{
    public static Task UploadFileAsync(
        this IFileStorage storage,
        byte[] content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        using MemoryStream memory = new(content);
        return storage.UploadFileAsync(memory, path, name, cancellationToken);
    }
}
