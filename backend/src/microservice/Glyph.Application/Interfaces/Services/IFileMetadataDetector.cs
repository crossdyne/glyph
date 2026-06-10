namespace Glyph.Application.Interfaces.Services
{
    public interface IFileMetadataDetector
    {
        Task<(string MimeType, string FormatName, string AssetTypeName)> DetectAsync(Stream stream, string fileName, CancellationToken ct);
    }
}