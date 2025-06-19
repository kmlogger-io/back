using System;

namespace Domain.Extensions;

public static class StreamExtensions
{
    public static string ToBase64(this Stream stream)
    {
        if (stream is null)
            throw new ArgumentNullException(nameof(stream));

        if (stream.CanSeek)
            stream.Position = 0;

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();
        return Convert.ToBase64String(bytes);
    }
}
