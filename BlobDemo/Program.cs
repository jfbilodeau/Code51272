// See https://aka.ms/new-console-template for more information
using System.Text;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;

Console.WriteLine("Blob storage demo");

var connectionString = "DefaultEndpointsProtocol=https;AccountName=demojfb;AccountKey=/GiI2CpHHEtM0r0JLIYSzx3OYQFvGYIJKt+J8Si1ZqCGftHwsOsSLQWrKkP3fS38PaUjP8HKGp33+AStirrqPg==;EndpointSuffix=core.windows.net";

var containerName = "demo";

var containerClient = new BlobContainerClient(connectionString, containerName);

await containerClient.CreateIfNotExistsAsync();

Console.WriteLine("Container created");

var blobName = "blockBlobDemo.txt";

{
    // This is a Block Blob client
    var blobClient = containerClient.GetBlobClient(blobName);

    Console.WriteLine("Check if blob exists");

    if (await blobClient.ExistsAsync())
    {
        Console.WriteLine("Blob exists");
    }
    else
    {
        Console.WriteLine("Blob does not exist");
        var content = "Hello, World!";

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        await blobClient.UploadAsync(stream, overwrite: true);
    }
}

{
    Console.WriteLine("Write a page blob");

    var pageBlobName = "pageBlobDemo.txt";

    var blobClient = containerClient.GetPageBlobClient(pageBlobName);

    Console.WriteLine($"Size of page: {blobClient.PageBlobPageBytes}");

    var buffer = new byte[blobClient.PageBlobPageBytes];

    var content = "Hello Page Blob!";

    var contentBytes = Encoding.UTF8.GetBytes(content);

    for (var i = 0; i < contentBytes.Length; i++)
    {
        buffer[i] = contentBytes[i];
    }

    blobClient.CreateIfNotExists(size: buffer.Length);

    using var stream = new MemoryStream(buffer);

    await blobClient.UploadPagesAsync(stream, offset: 0);

    Console.WriteLine("Page blob written");
}

{
    Console.WriteLine("Write an append blob");

    var appendBlobName = "appendBlobDemo.txt";

    var blobClient = containerClient.GetAppendBlobClient(appendBlobName);

    var content1 = "Hello Append Blob!";

    using var stream1 = new MemoryStream(Encoding.UTF8.GetBytes(content1));

    await blobClient.CreateIfNotExistsAsync();

    await blobClient.AppendBlockAsync(stream1);

    var content2 = "Hello again Append Blob!";

    using var stream2 = new MemoryStream(Encoding.UTF8.GetBytes(content2));

    await blobClient.AppendBlockAsync(stream2);

    Console.WriteLine("Append blob written");
}


Console.WriteLine("Done!");
