using Microsoft.Extensions.Logging;
using Orleans.Storage;
using System.IO.Compression;

namespace GzipSample.Shared;

public class GzipSerializer : IGrainStorageSerializer
{
    private readonly IGrainStorageSerializer _serializer;
    private readonly ILogger<GzipSerializer> _logger;

    public GzipSerializer(IGrainStorageSerializer serializer, ILogger<GzipSerializer> logger)
    {
        _serializer = serializer;
        _logger = logger;
    }

    public T Deserialize<T>(BinaryData input)
    {
        _logger.LogInformation($"{nameof(GzipSerializer)}: Deserializing input for type {typeof(T).Name}");

        using var inputStream = new MemoryStream(input.ToArray());
        using var decompressor = new GZipStream(inputStream, CompressionMode.Decompress);
        using var outputStream = new MemoryStream();

        decompressor.CopyTo(outputStream);

        return _serializer.Deserialize<T>(BinaryData.FromBytes(outputStream.ToArray()));
    }

    public BinaryData Serialize<T>(T input)
    {
        _logger.LogInformation($"{nameof(GzipSerializer)}: Serializing input for type {typeof(T).Name}");

        var serialized = _serializer.Serialize(input);

        using var inputStream = new MemoryStream(serialized.ToArray());

        using var outputStream = new MemoryStream();
        using var compressor = new GZipStream(outputStream, CompressionMode.Compress);
        inputStream.CopyTo(compressor);

        return BinaryData.FromBytes(outputStream.ToArray());
    }
}
