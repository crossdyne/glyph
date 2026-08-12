using Glyph.Assets.Application.Interfaces.Services;

namespace Glyph.Assets.Tests.Integration
{
    public class FakeMetadataDetector : IFileMetadataDetector
    {
        private readonly string _mimeType;
        private readonly string _formatName;
        private readonly string _assetTypeName;

        public FakeMetadataDetector(string formatName = ".svg", string assetTypeName = "Svg", string mimeType = "image/svg+xml")
        {
            _mimeType = mimeType;
            _formatName = formatName;
            _assetTypeName = assetTypeName;
        }

        public Task<(string MimeType, string FormatName, string AssetTypeName)> DetectAsync(Stream content, string fileName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult((_mimeType, _formatName, _assetTypeName));
        }
    }
}