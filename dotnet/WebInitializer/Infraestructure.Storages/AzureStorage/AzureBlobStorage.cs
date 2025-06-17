using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace Infraestructure.Storages.AzureStorage;

public class AzureBlobStorage(ILogger<AzureBlobStorage> logger, BlobServiceClient blobServiceClient)
    : IFileStorage
{
    public async Task<Stream> DownloadFileAsync(
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        BlobContainerClient? container = blobServiceClient.GetBlobContainerClient(path);
        BlobClient? blob = container.GetBlobClient(name);

        if (await blob.ExistsAsync(cancellationToken))
        {
            Response<BlobDownloadInfo>? response = await blob.DownloadAsync(cancellationToken);

            return response.Value.Content;
        }

        logger.LogError("Blob {name} does not exist", name);
        throw new FileNotFoundException($"The blob {name} doesn't exist");
    }

    public async Task UploadFileAsync(
        Stream content,
        string path,
        string name,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            BlobContainerClient? container = blobServiceClient.GetBlobContainerClient(path);
            BlobClient? blob = container.GetBlobClient(name);

            content.Position = 0;

            BlobUploadOptions options = new()
            {
                TransferOptions = new StorageTransferOptions { MaximumConcurrency = 10 },
            };

            await blob.UploadAsync(content, options, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error uploading file {name}", name);
            throw new Exception($"Error uploading file {name}", e);
        }
    }
}
