namespace Infraestructure.Storages;

public static class FileStorageExtensions
{
    public static async Task UploadFileAsync(
        this IFileStorage storage,
        byte[] content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        using MemoryStream memory = new(content);
        await storage.UploadFileAsync(memory, path, name, cancellationToken);
    }
}
