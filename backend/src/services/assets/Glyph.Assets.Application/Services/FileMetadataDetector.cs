using System.Buffers;
using System.Text;
using Glyph.Assets.Application.Interfaces.Services;

namespace Glyph.Assets.Application.Services
{
    public sealed class FileMetadataDetector : IFileMetadataDetector
    {
        public async Task<(string MimeType, string FormatName, string AssetTypeName)> DetectAsync(
            Stream stream, 
            string fileName, 
            CancellationToken ct)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(1024);

            try
            {
                int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, 1024), ct);

                if (stream.CanSeek) 
                    stream.Position = 0;

                if (bytesRead == 0)
                    throw new InvalidOperationException("Файл пуст.");

                var mimeType = DetectMimeType(buffer, bytesRead, fileName);
                var mapping = MapToDomain(mimeType, fileName);

                return (mimeType, mapping.FormatName, mapping.AssetTypeName);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private string DetectMimeType(byte[] buffer, int bytesRead, string fileName)
        {
            string header = Encoding.UTF8.GetString(buffer, 0, bytesRead).TrimStart();

            if (header.Contains("<svg", StringComparison.OrdinalIgnoreCase))
                return "image/svg+xml";

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream"
            };
        }

        private static (string FormatName, string AssetTypeName) MapToDomain(string mimeType, string fileName)
        {

            var known = new Dictionary<string, (string Format, string Type)>(StringComparer.OrdinalIgnoreCase)
            {
                { "image/svg+xml", (".svg", "Svg") }
            };

            if (known.TryGetValue(mimeType, out var mapping))
                return mapping;

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".svg" => (".svg", "Svg"),
                _ => throw new NotSupportedException($"Формат файла '{mimeType}' не поддерживается системой.")
            };
        }
    }
}